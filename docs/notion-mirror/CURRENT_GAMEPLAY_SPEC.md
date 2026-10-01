# Beast Link Battle — Current Gameplay Spec (Working Git Mirror)

Status: Design Source of Truth mirror.
Snapshot: 2026-10-01.
Notion source page: `3ec2674d-32c3-8122-96a8-de866c65c434`.

> Important: this file mirrors the last known adopted design state. Experimental Phaser variants do not become canonical rules unless explicitly adopted.

## P1 Core Loop

**Beast Rush → Energy Rush → Battle Setup / Beast Arrangement → Autonomous Battle + Timed Energy Cast → Result → Restart**

Key structural rule:
- Energy is collected before Battle.
- Active Battle contains no puzzle matching.
- Battle progresses autonomously.
- The player may spend finite stored Energy during Battle.

## Beast Rush

- Board: Beast-only 6×6 Onet board.
- Onet rule: Straight / L / Z / U-style routes with at most 2 bends / 3 segments, including logical outer-border routing.
- Successful Beast match adds +1 to the matched Beast ID in BeastQueue.
- Adopted Beast Combo baseline remains:
  - initial = 5.0s
  - bonus per valid match = +0.3s
  - cap = 5.0s
- Combo timing remains **Needs Validation**.
- P1-V2 and P1-V3 timing values are Experimental only and are not adopted here.

## Energy Rush

- Fresh Energy-only 6×6 Onet board.
- Energy is stored for later Battle use.
- Exact final EnergyRush termination rule remains an **Open Decision** in the adopted spec.
- Exact final Energy match → charge conversion remains an **Open Decision**.
- Current Phaser conversion `1 valid pair → +1 charge for matched Energy ID` is Experimental Variant A, not an adopted rule.
- P1-V1/V3 fixed Energy timers are Experimental only and are not adopted here.

## Battle Setup / Beast Arrangement

Beast roles:
- Tanker
- Assassin
- Ranger
- Mage

Adopted structural requirement:
- role readability matters before combat,
- player arranges Beasts before Battle,
- STAR and role should be visible enough to support placement decisions.

Still Experimental / unresolved:
- exact Beast ID → role mapping,
- exact role stats,
- exact formation topology,
- whether mid-combat reposition exists.

## STAR Conversion

Adopted baseline:
- 1 copy → 1★
- 3 copies → 2★
- 9 copies → 3★

STAR 1 / 3 / 9 is adopted, but its interaction with formation and role decisions still requires player validation.

## Autonomous Battle

Adopted structural requirement:
- Battle continues even with zero player input.
- Active Battle has no puzzle board.
- Formation must create visible consequences.

Still Experimental:
- role stats,
- star multipliers,
- enemy HP / damage,
- tick interval,
- targeting details,
- waves / multi-enemy behavior.

## Timed Energy Cast

Adopted structural requirement:
- Energy collected before Battle becomes finite Battle resources.
- player chooses when to spend Energy during autonomous combat.

Still Experimental:
- Frontline Heal,
- heal amount,
- mapping from Energy IDs to skills,
- final skill set,
- exact cast timing/balance.

## Validation Questions

- VQ-01: Can Beast Rush output produce an army the player can understand and arrange meaningfully?
- VQ-02: Are Beast roles readable enough to influence placement?
- VQ-03: Does Energy Rush clearly communicate stored later-use Energy?
- VQ-04: Does autonomous Battle create a clear consequence with no input?
- VQ-05: Does finite Energy create meaningful cast-now vs save-for-later timing?
- VQ-06: Does STAR 1/3/9 interact with role/formation choice understandably?

## Change Rule

Use:

`Validation Finding → Design Proposal → Experimental Variant → Retest → Decision → Adopt / Reject`

Only **Adopted** decisions update this file.
