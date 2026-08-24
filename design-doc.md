# Demon Lord — Core Game Design Document

## 1. High Concept

A strategy/management game in which the player assumes the role of a newly awakened **Demon Lord** attempting to conquer a fantasy kingdom.

The player:

* establishes a demonic tower from scratch;
* builds rooms and assigns monsters to defend them;
* performs evil deeds throughout the kingdom to acquire power and provoke increasingly powerful heroes;
* manipulates the motivations of invading hero parties;
* fights those parties primarily **inside the tower**;
* develops a roster of disposable minions, persistent monsters, and powerful champions;
* ultimately attempts to survive long enough to become the dominant power in the kingdom.

The central fantasy is not:

> "Build a tower and watch enemies die in traps."

Nor:

> "Conquer the kingdom in a 4X."

Nor:

> "Play an RPG party of monsters."

Instead:

> **Build a fortress, lure heroes into it, dismantle their carefully constructed party tactics, bleed them of resources, and decide when to sacrifice your monsters and when to preserve them.**

---

# 2. Core Design Pillars

### 2.1 The tower is the main battlefield

Most combat happens inside the Demon Lord's tower.

The world map exists primarily to:

* generate consequences;
* provide resources and power;
* create threats and objectives;
* determine which heroes eventually attack the tower.

The game should not become a conventional 4X where the player spends most of their time conquering territory with armies.

---

### 2.2 The tower is vertical, but not a 3D dungeon

The tower consists of **floors**, rather than one large 2D labyrinth.

Each floor contains a small number of rooms, normally **2–4**, with three being typical.

The tower therefore looks conceptually like:

```text
             FLOOR 6
       [Vault] [Throne] [Crypt]

             FLOOR 5
    [Chapel] [Library] [Armory]

             FLOOR 4
       [Garden] [Forge]

             FLOOR 3
    [Prison] [Hall] [Laboratory]

             FLOOR 2
       [Barracks] [Pit]

             FLOOR 1
          [Entrance]
```

There is no need for conventional grid-based movement.

The tower's topology is represented abstractly.

---

### 2.3 Heroes are invading adventurers, not dungeon-clearing robots

A hero party enters the tower with a **specific objective**.

Examples:

* kill the Demon Lord;
* recover a stolen relic;
* destroy a ritual;
* rescue prisoners;
* recover forbidden knowledge;
* destroy a particular room;
* investigate the tower;
* retrieve an artifact and escape.

They should not automatically want to clear every room.

This prevents the game from becoming a conventional dungeon-crawler.

---

### 2.4 Combat is about breaking plans

Combat does not primarily revolve around moving units across a tactical grid.

Instead, combat is based on:

* intentions;
* relationships;
* conditions;
* behavioral states;
* setups;
* reactions;
* tactical chains.

A hero party has an interlocking plan.

The Demon Lord attempts to break it.

Example:

```text
Paladin → Protect Wizard
Wizard → Cast Spell
Rogue → Steal Relic
```

The Demon Lord might use:

```text
Cultist → Taunt Paladin
```

causing:

```text
Paladin → Attack Cultist
```

which breaks:

```text
Paladin → Protect Wizard
```

allowing:

```text
Bone Knight → Bone Spear → Wizard
```

The interesting decision is therefore:

> **Which relationship or intention do I disrupt first?**

rather than:

> Which unit should I move three squares?

---

# 3. Tower Structure

## 3.1 Floors

A floor is a **small decision space**.

The heroes arrive at the floor and are presented with its rooms.

For example:

```text
FLOOR 4

     [Blood Chapel]
          │
[Library] │ [Armory]
          │
       Stairs
```

The actual physical connections need not be simulated.

The important information is:

* which rooms exist;
* what they contain;
* what they appear to offer;
* which room the heroes choose.

---

## 3.2 Room types

Rooms have several properties:

* **Theme**
* **Capacity**
* **Special rules**
* **Visible attraction**
* **Hidden information**
* **Available guardians**
* **Potential objective**
* **Special hero interactions**

Examples:

### Blood Chapel

* strong vampire synergy;
* demonic/holy conflict;
* attracts Paladins;
* contains blood-related hazards.

### Library

* magical knowledge;
* attractive to Wizards;
* may reveal information about the Demon Lord.

### Armory

* valuable equipment;
* attractive to Rogues and martial heroes;
* potentially useful loot.

### Laboratory

* dangerous magical experiments;
* potentially high reward;
* Wizard attraction.

---

# 4. Hero Room Selection

This is one of the game's key strategic systems.

The Demon Lord **does not directly choose the room the heroes enter**.

Instead, the player manipulates the circumstances and predicts their decision.

Heroes evaluate rooms based on:

* their current objective;
* party composition;
* individual impulses;
* known information;
* perceived danger;
* potential rewards;
* personalities;
* clues gathered during the expedition.

---

## 4.1 Hero impulses

Individual heroes have behavioral preferences.

Examples:

### Paladin

**Destroy Evil**

Prefers strongly corrupted or demonic locations.

### Rogue

**Acquire Treasure**

Prefers rooms likely to contain valuables.

### Wizard

**Understand Magic**

Prefers magical or mysterious locations.

### Ranger

**Explore the Unknown**

Prefers hidden, natural, or unexplored areas.

These impulses influence party decisions.

The party then has to reconcile competing preferences.

---

## 4.2 The Demon Lord manipulates those impulses

The player can make certain rooms more attractive.

For example:

> A powerful magical signature appears to come from the Library.

The Wizard wants the Library.

Or:

> A legendary artifact is visibly stored in the Vault.

The Rogue wants the Vault.

Or:

> The Chapel radiates unmistakable evil.

The Paladin wants the Chapel.

The Demon Lord is therefore engaging in **psychological warfare**.

---

## 4.3 Predictable rather than arbitrary AI

The game should make hero decisions understandable.

Ideally the player can think:

> "The Paladin will probably choose the Chapel, unless the Rogue's desire for the artifact wins the argument."

The UI could optionally communicate something like:

```text
LIKELY HERO CHOICE

Chapel       55%
Vault        30%
Library      15%
```

This isn't necessarily a literal probability system; it can be a prediction/intent UI.

The important principle is:

> **The player should be able to reason about hero behavior.**

---

# 5. Room Accessibility

Rooms can have different accessibility states.

### Open

Any normal party can enter.

### Sealed

Requires a specific hero ability.

### Hidden

The party doesn't initially know the room exists.

### Blocked

Requires a specific action or capability.

This gives hero abilities utility outside combat.

Examples:

* Rogue opens sealed doors.
* Wizard detects magical rooms.
* Ranger discovers hidden routes.
* Paladin breaks holy barriers.

Importantly, this does not require actual pathfinding.

---

# 6. Guardian Deployment

Before the heroes choose a room, the Demon Lord gets an opportunity to arrange the defenders on that floor.

Example:

```text
FLOOR 5

Chapel:   Vampire + Cultist
Library:  Bone Knight
Armory:   Ogre + Cultist
Reserve:  Demon General
```

The player knows:

* the hero party;
* their current condition;
* their likely preferences;
* their objective.

The player then makes a prediction:

> "Where are they going?"

and deploys accordingly.

---

## 6.1 Deployment locks after the choice

The order is:

```text
Heroes arrive
      ↓
Demon Lord deploys guardians
      ↓
Heroes choose room
      ↓
Deployment locks
      ↓
Combat begins
```

The player cannot simply wait for the heroes to choose and then move the entire army into the selected room.

This makes prediction meaningful.

---

# 7. Combat Entry and Reinforcements

The first combat round represents the **breach**.

### Round 1

Only monsters stationed in the chosen room can act normally.

This makes deployment important.

### Round 2 onward

Other monsters stationed elsewhere on the floor can arrive as reinforcements.

The exact reinforcement rules should remain simple.

For example:

> One reinforcement can enter per round.

Some monsters may have special abilities allowing them to arrive faster.

Examples:

* teleportation;
* flight;
* tunnels;
* mist form;
* summoning.

This creates a natural escalation:

```text
BREACH
   ↓
Initial defenders
   ↓
ALARM
   ↓
Reinforcements
   ↓
Escalation
   ↓
Resolution
```

---

# 8. Floor Resolution

This is a critical simplification.

> **Only one room on a floor can be cleared during a hero expedition.**

Once the heroes successfully resolve the room they chose, that room becomes **cleared for this expedition**.

The heroes then proceed upward.

They do not get to systematically visit every room and eliminate all the defenders.

Example:

```text
FLOOR 4

[Library] [Chapel] [Armory]
    ✓
```

If the heroes choose Library and defeat its defenders:

> Library is cleared.

Chapel and Armory remain unresolved.

The party proceeds upward.

This avoids the "clean the whole floor" problem without requiring additional stamina, tempo, alarm, or other resource systems.

---

# 9. Between-Floor Redeployment

When the heroes climb to the next floor, the Demon Lord gets another deployment opportunity.

The player may rearrange guardians freely across the new floor.

Additionally, there is **one opportunity to rearrange available monsters between floors**, allowing the player to react to the expedition's progress and prevent all surviving demons from automatically converging on the hero party.

This creates a simple strategic rhythm:

```text
Floor N
 ↓
Deploy
 ↓
Heroes choose
 ↓
Fight
 ↓
One room cleared
 ↓
Heroes ascend
 ↓
Redeploy
 ↓
Floor N+1
```

No continuous army-management simulation is required.

---

# 10. Monster Persistence

Monsters should not all have the same relationship with death.

## 10.1 Disposable monsters

Examples:

* Cultists
* Imps
* Skeletons
* Lesser beasts

These are relatively cheap and replenishable.

Their purpose is to:

* create tactical openings;
* apply conditions;
* disrupt hero plans;
* buy time;
* sacrifice themselves.

Their death is acceptable.

A Cultist dying after successfully taunting the Paladin may be considered a successful tactical exchange.

---

## 10.2 Veteran monsters

Examples:

* Bone Knights
* Vampires
* Ogres
* Demon assassins

These are persistent characters.

They can be wounded and retreat.

Their injuries persist between encounters or expeditions.

They are valuable enough that the player has to decide:

> Do I risk this unit for a better tactical result?

---

## 10.3 Champions

Examples:

* Demon General
* Vampire Lord
* Greater Demon
* named lieutenants

These are campaign-level characters.

Their deaths are major events.

They should be difficult to replace and potentially have unique relationships with the tower.

---

# 11. Retreat

Retreat should be a normal tactical option for important monsters.

A veteran monster should often be able to withdraw rather than fight to the death.

Different monsters can have different retreat behavior.

### Cowardly

Withdraws when seriously wounded.

### Disciplined

Withdraws only when badly wounded.

### Fanatical

Doesn't voluntarily retreat.

### Berserker

Retreats only after fulfilling a condition.

### Guardian

Cannot retreat while protecting a specific objective.

This can be represented as simple monster behavior rather than complicated AI.

---

# 12. Monster Recovery

Replenishment happens primarily **between expeditions**, rather than continuously during combat.

The tower's infrastructure determines recovery.

Examples:

### Demon Foundry

Produces disposable demons.

### Barracks

Houses veteran creatures.

### Blood Chamber

Heals vampires.

### Necromantic Laboratory

Reanimates undead.

### Soul Forge

Repairs/reconstitutes powerful demons.

This makes tower construction partly a **military logistics system**.

---

# 13. Combat System

The combat system is deliberately abstract.

There is no requirement for:

* grid movement;
* facing;
* pathfinding;
* cover calculations;
* line-of-sight simulation;
* spatial AI.

Instead, combatants have:

* objectives;
* targets;
* intentions;
* relationships;
* conditions;
* triggers;
* abilities.

---

## 13.1 Hero plans

A hero party has a visible or inferable tactical plan.

Example:

```text
PALADIN
Protect → Wizard

WIZARD
Cast → Sanctification

ROGUE
Steal → Relic
```

This creates a network of dependencies.

---

## 13.2 Monster abilities

Monsters possess relatively simple actions that manipulate those dependencies.

Example:

### Cultist

**Blasphemy**

Taunts a hero.

### Bone Knight

**Bone Spear**

Deals devastating damage to an exposed target.

### Vampire

**Scent Blood**

Changes behavior when an enemy becomes wounded.

These actions interact.

---

# 14. Combat as Setup and Payoff

A large portion of monster design should follow:

> **Setup → Exploitation → Payoff**

Example:

```text
Cultist
  ↓
Taunt Paladin
  ↓
Paladin abandons Wizard
  ↓
Wizard becomes Exposed
  ↓
Bone Knight uses Bone Spear
  ↓
Wizard becomes Wounded
  ↓
Vampire senses blood
  ↓
Vampire changes target
```

The player is effectively manipulating a tactical machine.

---

# 15. Combat Duration

Initial target:

### **3–5 rounds per combat encounter**

The system should be designed so that fights rarely continue simply because both sides still have HP.

Combat can end because:

* heroes achieve their objective;
* demons achieve their objective;
* a key character dies;
* heroes retreat;
* demons retreat;
* a room becomes untenable;
* a special event occurs;
* an important timer/condition is completed.

The goal is a **dynamic tactical sequence**, rather than a prolonged damage race.

---

# 16. Hero Attrition

The Demon Lord does not have to defeat the heroes on every floor.

The important question is:

> **How much does the party pay to progress?**

Heroes can accumulate:

* wounds;
* exhaustion;
* curses;
* lost equipment;
* spent abilities;
* dead party members;
* other persistent disadvantages.

Therefore:

> **The Demon Lord can lose a battle while winning the expedition.**

Example:

### Floor 2

Heroes defeat defenders.

Cost:

* Paladin wounded.

### Floor 3

Heroes defeat defenders.

Cost:

* Wizard exhausted;
* Rogue cursed.

### Floor 4

Heroes finally reach the major defensive position.

They are now significantly weaker than when they entered.

---

# 17. The Target Floor

Lower floors and the target floor serve different strategic purposes.

### Lower floors

The heroes have choices.

The Demon Lord attempts to:

* manipulate their impulses;
* predict their route;
* force unfavorable fights;
* extract wounds;
* consume their abilities;
* sacrifice disposable demons efficiently.

### Target floor

The party has a specific destination.

For example:

> **Destroy the Soul Forge.**

The relevant room is known.

The heroes will go there.

The Demon Lord therefore knows exactly where the confrontation will happen.

This creates a transition from:

> **Psychological warfare**

to:

> **Direct tactical defense.**

---

# 18. The Throne

The final floor contains the Demon Lord's actual throne/chamber.

The ultimate hero objective may be:

> **Kill the Demon Lord.**

Unlike lower floors, the player cannot redirect the party.

The entire campaign has been building toward this confrontation.

The Demon Lord's accumulated:

* tower design;
* monster roster;
* surviving champions;
* resources;
* previous choices;
* knowledge of the heroes

all culminate here.

---

# 19. The World Map

The kingdom exists primarily as a **source of strategic consequences**, rather than the main battlefield.

The Demon Lord performs evil deeds such as:

* corrupting villages;
* attacking caravans;
* spreading cults;
* stealing artifacts;
* manipulating kingdoms;
* awakening monsters;
* destroying infrastructure;
* corrupting magical sites.

These actions generate:

* power;
* resources;
* new monsters;
* tower construction opportunities;
* reputation/threat;
* hero responses.

The more disruptive the Demon Lord becomes, the more serious the heroes sent against them become.

---

# 20. The Core Campaign Loop

```text
        WORLD
          │
          ▼
   Perform Evil Deeds
          │
          ▼
   Gain Power / Threat
          │
          ▼
    Heroes Respond
          │
          ▼
     Hero Party Forms
          │
          ▼
    Tower Expedition
          │
          ▼
    ┌─────────────┐
    │    FLOOR    │
    │             │
    │ Deploy      │
    │     ↓       │
    │ Heroes      │
    │ choose      │
    │     ↓       │
    │ Combat      │
    │     ↓       │
    │ One room    │
    │ cleared     │
    └──────┬──────┘
           │
           ▼
       Next Floor
           │
           ▼
      Redeploy
           │
           ▼
        Repeat
           │
           ▼
      Target Floor
           │
           ▼
      Throne Fight
           │
           ▼
   Campaign Consequences
           │
           └──────────► WORLD
```

---

# 21. What Makes This Different

The game's strategic problem is not:

> **"Can I build an impenetrable tower?"**

Nor:

> **"Can I kill every hero?"**

Nor:

> **"Can I conquer the kingdom?"**

It is:

> **"Can I manipulate this particular party into fighting the battles I want, in the places I want, while dismantling their tactical cooperation faster than they dismantle my defenses?"**

The player is simultaneously managing three things:

### The Tower

Where will the confrontation happen?

### The Monsters

Who should be committed, sacrificed, preserved, or reinforced?

### The Heroes

What do they want, and how can I make those desires work against them?

---

# 22. Prototype Scope

A first playable prototype could be remarkably small.

### Tower

* 5 floors
* 3 rooms per floor
* 1 target room
* simple room tags

### Heroes

* 4 hero classes
* 3–4 heroes per party
* simple impulses
* simple objectives

### Monsters

* 4–6 monster types
* 2 disposable
* 2 veteran
* 1 champion

### Combat

* 3–5 rounds
* conditions
* targets
* intentions
* 2–4 abilities per creature
* reinforcement on round 2

### Campaign

* basic world map
* 5–10 evil deeds
* simple power/threat progression
* hero party generation

This should be enough to test the **actual core hypothesis**:

> **Is manipulating a hero party's plan and choosing where to deploy monsters fun enough to carry the game?**

If that works, the rest of the game can be built around it.
