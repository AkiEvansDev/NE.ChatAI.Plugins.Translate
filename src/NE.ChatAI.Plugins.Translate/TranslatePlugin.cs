using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NE.ChatAI.Plugins.Translate;

/// <summary>Entry point for the official <c>Translate</c> scenario plugin.</summary>
public sealed class TranslatePlugin : IChatPlugin
{
    /// <inheritdoc />
    public string Id => "ne-chatai-translate";

    /// <inheritdoc />
    public IReadOnlyList<Type> NodeTypes { get; } =
    [
        typeof(TranslateTextNode)
    ];
}

/// <summary>Translates wired scenario text or the active composer draft into a target language (stub implementation).</summary>
[GraphNode(Key = NodeKey, Category = "Translate", Title = "Translate text", Description = "Translates scenario text or the composer draft into the target language (stub).", Icon = UIGlyphs.Replace, Color = "var(--ui-color-info)", ShowProgress = true)]
public sealed class TranslateTextNode : IGraphNodeAsync
{
    /// <summary>Stable scenario node key registered by <see cref="TranslateTextNode"/>.</summary>
    public const string NodeKey = "translate.text";

    /// <summary>Gets or sets the input text to translate; falls back to the active composer draft when empty.</summary>
    [GraphInput(Title = "Text", MaxLines = 3, Placeholder = "Optional text; falls back to the composer draft")]
    public string Text { get; set; } = string.Empty;

    /// <summary>Gets or sets the source language code (<c>auto</c> for automatic detection).</summary>
    [GraphInput(Title = "Source", MaxLength = 16, Placeholder = "auto")]
    public string SourceLanguage { get; set; } = "auto";

    /// <summary>Gets or sets the target language code.</summary>
    [GraphInput(Title = "Target", MaxLength = 16, Placeholder = "en")]
    public string TargetLanguage { get; set; } = "en";

    /// <summary>Gets or sets the optional secret slug in <c>Settings -&gt; Secrets</c> holding an external provider API key.</summary>
    [GraphInput(Title = "Secret", NoPin = true, MaxLength = 80, Placeholder = "translate-api-key", Description = "Optional secret slug in Settings -> Secrets for external translation providers.")]
    public string SecretName { get; set; } = "translate-api-key";

    /// <summary>Gets or sets the translated output text.</summary>
    [GraphOutput(Title = "Text")]
    public string TranslatedText { get; set; } = string.Empty;

    /// <summary>Gets or sets the normalized source language code.</summary>
    [GraphOutput(Title = "Source")]
    public string DetectedLanguage { get; set; } = string.Empty;

    /// <summary>Gets or sets a value indicating whether the configured secret slug currently resolves to a stored API key.</summary>
    [GraphOutput(Title = "Secret ready")]
    public bool HasSecret { get; set; }

    /// <inheritdoc />
    public async ValueTask ExecuteAsync(UINodeRunContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        IScenarioContext scenario = context.GetScenarioContext();
        IScenarioSecrets secrets = context.GetScenarioSecrets();

        var secretSlug = SecretName.Trim();
        HasSecret = secretSlug.Length > 0 && !string.IsNullOrEmpty(await secrets.GetAsync(secretSlug, cancellationToken).ConfigureAwait(false));

        var source = string.IsNullOrWhiteSpace(SourceLanguage) ? "auto" : SourceLanguage.Trim().ToLowerInvariant();
        var target = string.IsNullOrWhiteSpace(TargetLanguage) ? "en" : TargetLanguage.Trim().ToLowerInvariant();
        DetectedLanguage = source;

        var input = string.IsNullOrWhiteSpace(Text) ? scenario.Draft.Trim() : Text.Trim();

        if (input.Length == 0)
        {
            TranslatedText = string.Empty;
            return;
        }

        await context.ReportAsync($"{source} -> {target}", 1.0).ConfigureAwait(false);
        await context.LogAsync($"Stub translation ({source} -> {target}, secretReady={HasSecret}).").ConfigureAwait(false);

        TranslatedText = string.Equals(source, target, StringComparison.OrdinalIgnoreCase)
            ? input
            : $"[{source}->{target}] {input}";
    }
}
