docfx version 2.40.8.0 does not support attributes on <inheritdoc/> tags

This well-formed tag:  /// <inheritdoc cref="Boolean.ToString()"/> is thus ignored by docfx 2.40.8.0

For now, avoid using properties inside inheritdoc tags.



Warnings are only emitted on full builds, incremental builds often hide extant warnings.

docfx docfx_project\docfx.json build --force does a full rebuild including all warnings.
script BuildServe-HelpDocumentation.ps1 performs an incremental build and serves up the documentation on localhost:8080





