# NE.ChatAI.Plugins.Translate

Official translation scenario plugin for [`NE.ChatAI`](https://github.com/AkiEvansDev/NE.ChatAI) (currently a stub demonstrating plugin installation, hot-updates, composer draft translation, and `IScenarioSecrets` lookup).

## Installation

Open **`/plugins`** in `NE.ChatAI`, enter:

```text
AkiEvansDev/NE.ChatAI.Plugins.Translate
```

and click **+** (or press **Enter**).

## Nodes

| Key | Category | Title | Description |
| --- | --- | --- | --- |
| `translate.text` | `Translate` | `Translate text` | Translates wired scenario text or the current composer draft from `Source` (`auto` by default) to `Target` (`en` by default), checking an optional secret slug in **Settings -> Secrets**. |

### Typical `BeforeSend` pipeline

1. Set the scenario trigger to **Before sending** (`BeforeSend`).
2. Wire **Draft** (`chat.draft-input`, or leave `Translate text`'s `Text` input unwired so it reads `IScenarioContext.Draft` directly) -> **Translate text** (`translate.text`, `Target = "en"`) -> **Write draft** (`chat.draft`, `Mode = Replace`).

## Building from source

Requires the .NET 11 SDK pinned in [`global.json`](global.json) and [`NE.ChatAI.Plugins`](https://www.nuget.org/packages/NE.ChatAI.Plugins) from NuGet:

```sh
dotnet build --configuration Release
```

## Contributing

This repository is a **read-only mirror** generated from a private monorepo. Pull requests are disabled here, while issues and feature requests are open and welcome.
