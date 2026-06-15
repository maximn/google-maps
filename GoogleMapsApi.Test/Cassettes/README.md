# Cassettes

Recorded Google API responses that back the integration tests in **replay** mode (the default), so the
suite runs offline, with no API key, and at no cost. See [`.agents/testing.md`](../../.agents/testing.md)
for the full record/replay workflow.

## Layout

```
Cassettes/<FixtureClassName>/<TestName>.json
```

One JSON file per test, resolved by `Vcr/CassetteLocator.cs`. Each file is an array of recorded
interactions (one per HTTP call the test makes, in order):

```jsonc
[
  {
    "Method": "GET",
    "Url": "https://maps.googleapis.com/maps/api/geocode/json?address=...&key=REDACTED",
    "RequestBody": null,                 // normalized JSON for POST calls; null for GET
    "StatusCode": 200,
    "ContentType": "application/json; charset=UTF-8",
    "BodyBase64": "..."                  // base64 — covers JSON and binary (GeoTIFF/photo) bodies
  }
]
```

## Rules

- **Committed to git.** They are the offline fixtures; do not gitignore them.
- **No secrets.** The `key`/`signature` query parameters are redacted to `REDACTED` on record, and
  matching redacts the incoming request the same way — so a recording made with a real key replays
  against the placeholder key used in replay mode.
- **(Re)record with:** `VCR_MODE=record GOOGLE_API_KEY=<key> RUN_BILLABLE_TESTS=true dotnet test`
  (optionally `--filter "ClassName=<Fixture>"` for one fixture), then commit the changed JSON.
- A missing cassette file makes its test `Ignore` (record it); a request missing inside an existing
  cassette fails the test loudly (Google API drift — re-record).
