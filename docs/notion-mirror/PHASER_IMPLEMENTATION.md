# Phaser Validation Prototype — Implementation Mirror

Status: implementation truth mirror.
Snapshot: 2026-10-01.
Notion matrix: `3ec2674d-32c3-8191-9709-d2b17efb3f57`.

## Current structural implementation

Implemented P1 flow:

**BeastRush → EnergyRush → BattleSetup → Battle → Result → BeastRush**

Implemented properties:
- BeastRush uses Beast-only 6×6 Onet.
- EnergyRush uses a fresh Energy-only 6×6 board.
- BattleSetup has no puzzle input.
- Battle has no puzzle input.
- Stored Energy persists into Battle.
- Restart clears per-run queues / formation / validation state.

## P1-S1 — Energy Pre-Collection

Experimental Variant A:
- 1 valid Energy pair → +1 stored charge for matched Energy ID.
- EnergyQueue persists through BattleSetup and Battle.
- Restart clears EnergyQueue.

## P1-S2 — Beast Role + Arrangement

Experimental placeholder role mapping:
- beast-a → Tanker
- beast-b → Assassin
- beast-c → Ranger
- beast-d → Mage
- beast-e → Tanker
- beast-f → Ranger

Formation:
- 3 rows × 6 columns
- Front / Mid / Back
- one unit per slot
- all units must be placed before Start Battle
- role recommendations are guidance, not restrictions

## P1-S3 — Autonomous Battle

Experimental fixtures:
- Tanker: HP 80 / damage 6
- Assassin: HP 35 / damage 14
- Ranger: HP 45 / damage 10
- Mage: HP 40 / damage 9
- star multipliers: 1★ ×1.0, 2★ ×1.8, 3★ ×3.2
- enemy HP 150
- enemy damage 15 per tick
- tick = 1 second

Targeting:
- Front → Mid → Back
- lowest column first within a row

Terminal Win/Lose states stop further combat and enter Result.

## P1-S4 — Timed Energy Cast

Experimental skill:
- Frontline Heal
- consumes 1 selected Energy charge
- heals current front-most alive unit +30 HP
- capped at max HP
- does not revive dead units
- Battle continues before and after casts

## P1-S5 — Validation Instrumentation

Tracked metrics:
- beastMatches
- energyMatches
- beastQueueAtSetup total / per ID
- energyChargesAtBattleStart total / per ID
- roleCounts
- starTierCounts
- arrangementChanges
- timeInBattleSetup
- firstCastTime
- castsUsed
- unusedChargesAtResult
- battleDuration
- resultWinLose
- armyHpAtFirstCast
- enemyHpAtFirstCast

P1-S5 was closed by project-owner sign-off. Historical verification debt remains documented for the final tiny check-file addition.

## Experimental timing variants

### P1-V1 — Pre-Battle Phase Clarity
- Beast Rush canonical baseline preserved: 5.0 / +0.3 / 5.0
- 1.0s non-interactive Energy Rush transition cue
- EnergyRush fixed 8.0s countdown
- timeout auto-enters BattleSetup
- automated checks + build + live verification passed

### P1-V2 — Beast Rush Timing
- explicit ComboSystem override:
  - 8.0s initial
  - +0.3s per valid Beast match
  - 8.0s cap
- Energy side remained P1-V1 1.0s cue + 8.0s countdown
- automated checks + build passed
- live tuning observation: Beast and Energy windows still felt too short

### P1-V3 — Extended Pre-Battle Timing
Current validation build:
- Beast Rush override = 12.0 / +0.3 / 12.0
- Energy cue = 1.0s
- EnergyRush countdown = 12.0s
- Energy conversion remains +1 charge per valid pair
- canonical RuleConfig remains 5.0 / +0.3 / 5.0
- all other P1 systems unchanged
- deterministic P1-V3 checks pass
- retained regressions pass
- production build passes
- project owner confirmed live pass

P1-V3 remains **Experimental** and is ready for P03 player validation.

## Sync discipline

Do not change Current Gameplay Spec to match this implementation unless a design decision is explicitly adopted.
