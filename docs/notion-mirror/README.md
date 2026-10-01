# Beast Link Battle — Project Docs Mirror

Status: active Git working mirror while Notion MCP access is unavailable.
Snapshot date: 2026-10-01.

## Why this folder exists

The Beast Link Battle design/validation documentation was maintained in Notion. Notion MCP is currently returning `UNAVAILABLE / internal_server_error`, so Git is now the operational communication layer for project state and design changes.

Until Notion access returns:

1. Update these Git docs first.
2. Treat Git commits as the handoff/change history.
3. Do not silently rewrite adopted design rules from implementation behavior.
4. Keep Experimental variants explicitly labeled.
5. When Notion access returns, back-sync Git deltas into the mapped Notion pages and mark them synced.

## Source hierarchy during the outage

1. `CURRENT_GAMEPLAY_SPEC.md` — current intended gameplay baseline.
2. `PHASER_IMPLEMENTATION.md` — what the Phaser validation prototype actually implements.
3. `VALIDATION_LOG.md` — player evidence, findings, variants, and decisions.
4. `PROJECT_HANDOFF.md` — current milestone, blocker, and next action.
5. Unity audit remains implementation truth for the Unity project and must not override the gameplay spec.

## Status vocabulary

- Implemented
- Partially Integrated
- Configured Prototype
- Experimental
- Needs Validation
- Needs Sync
- Synced
- Conflict

## Change discipline

Use:

`Validation Finding → Design Proposal → Experimental Variant → Retest → Decision → Adopt / Reject`

Only an adopted decision updates the Current Gameplay Spec. Experimental Phaser values do not become design truth merely because they are implemented.

## Notion page mapping

- Current Gameplay Spec: `3ec2674d-32c3-8122-96a8-de866c65c434`
- Unity Implementation Audit: `3ec2674d-32c3-811d-a2f7-dc1c6528cb9b`
- Phaser Validation Prototype Spec: `3ec2674d-32c3-8155-a07e-c449ad9f9ea2`
- Phaser Implementation Matrix: `3ec2674d-32c3-8191-9709-d2b17efb3f57`
- Validation Log: `3ec2674d-32c3-81da-8c14-df447e09cb92`
- Project Handoff: `3ec2674d-32c3-8180-a291-f5e3b51fadf4`
- Figma Sync Spec: `3ec2674d-32c3-8166-b437-f44754ae4eb7`

## Current project state

Current validation build: Experimental P1-V3.

Pre-Battle timing:
- Beast Rush: 12.0s initial / +0.3s per valid match / 12.0s cap.
- Energy transition cue: 1.0s.
- Energy Rush: 12.0s.
- Energy conversion: +1 charge per valid Energy pair.

P1-V3 automated checks/build pass and the project owner confirmed the live pass. Next milestone is P03 real-player validation.

Figma remains Needs Sync / Deferred and must not be used as current UX/portfolio evidence until refreshed.
