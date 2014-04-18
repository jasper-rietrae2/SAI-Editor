using System;

namespace SAI_Editor.Enumerators
{

    public enum SmartEvent
    {
        SMART_EVENT_UPDATE_IC = 0,
        SMART_EVENT_UPDATE_OOC = 1,
        SMART_EVENT_HEALT_PCT = 2,
        SMART_EVENT_MANA_PCT = 3,
        SMART_EVENT_AGGRO = 4,
        SMART_EVENT_KILL = 5,
        SMART_EVENT_DEATH = 6,
        SMART_EVENT_EVADE = 7,
        SMART_EVENT_SPELLHIT = 8,
        SMART_EVENT_RANGE = 9,
        SMART_EVENT_OOC_LOS = 10,
        SMART_EVENT_RESPAWN = 11,
        SMART_EVENT_TARGET_HEALTH_PCT = 12,
        SMART_EVENT_VICTIM_CASTING = 13,
        SMART_EVENT_FRIENDLY_HEALTH = 14,
        SMART_EVENT_FRIENDLY_IS_CC = 15,
        SMART_EVENT_FRIENDLY_MISSING_BUFF = 16,
        SMART_EVENT_SUMMONED_UNIT = 17,
        SMART_EVENT_TARGET_MANA_PCT = 18,
        SMART_EVENT_ACCEPTED_QUEST = 19,
        SMART_EVENT_REWARD_QUEST = 20,
        SMART_EVENT_REACHED_HOME = 21,
        SMART_EVENT_RECEIVE_EMOTE = 22,
        SMART_EVENT_HAS_AURA = 23,
        SMART_EVENT_TARGET_BUFFED = 24,
        SMART_EVENT_RESET = 25,
        SMART_EVENT_IC_LOS = 26,
        SMART_EVENT_PASSENGER_BOARDED = 27,
        SMART_EVENT_PASSENGER_REMOVED = 28,
        SMART_EVENT_CHARMED = 29,
        SMART_EVENT_CHARMED_TARGET = 30,
        SMART_EVENT_SPELLHIT_TARGET = 31,
        SMART_EVENT_DAMAGED = 32,
        SMART_EVENT_DAMAGED_TARGET = 33,
        SMART_EVENT_MOVEMENTINFORM = 34,
        SMART_EVENT_SUMMON_DESPAWNED = 35,
        SMART_EVENT_CORPSE_REMOVED = 36,
        SMART_EVENT_AI_INIT = 37,
        SMART_EVENT_DATA_SET = 38,
        SMART_EVENT_WAYPOINT_START = 39,
        SMART_EVENT_WAYPOINT_REACHED = 40,
        SMART_EVENT_TRANSPORT_ADDPLAYER_NYI = 41,
        SMART_EVENT_TRANSPORT_ADDCREATURE_NYI = 42,
        SMART_EVENT_TRANSPORT_REMOVE_PLAYER_NYI = 43,
        SMART_EVENT_TRANSPORT_RELOCATE_NYI = 44,
        SMART_EVENT_INSTANCE_PLAYER_ENTER_NYI = 45,
        SMART_EVENT_AREATRIGGER_ONTRIGGER = 46,
        SMART_EVENT_QUEST_ACCEPTED_NYI = 47,
        SMART_EVENT_QUEST_OBJ_COPLETETION_NYI = 48,
        SMART_EVENT_QUEST_COMPLETION_NYI = 49,
        SMART_EVENT_QUEST_REWARDED_NYI = 50,
        SMART_EVENT_QUEST_FAIL_NYI = 51,
        SMART_EVENT_TEXT_OVER = 52,
        SMART_EVENT_RECEIVE_HEAL = 53,
        SMART_EVENT_JUST_SUMMONED = 54,
        SMART_EVENT_WAYPOINT_PAUSED = 55,
        SMART_EVENT_WAYPOINT_RESUMED = 56,
        SMART_EVENT_WAYPOINT_STOPPED = 57,
        SMART_EVENT_WAYPOINT_ENDED = 58,
        SMART_EVENT_TIMED_EVENT_TRIGGERED = 59,
        SMART_EVENT_UPDATE = 60,
        SMART_EVENT_LINK = 61,
        SMART_EVENT_GOSSIP_SELECT = 62,
        SMART_EVENT_JUST_CREATED = 63,
        SMART_EVENT_GOSSIP_HELLO = 64,
        SMART_EVENT_FOLLOW_COMPLETED = 65,
        SMART_EVENT_DUMMY_EFFECT_NYI = 66,
        SMART_EVENT_IS_BEHIND_TARGET = 67,
        SMART_EVENT_GAME_EVENT_START = 68,
        SMART_EVENT_GAME_EVENT_END = 69,
        SMART_EVENT_GO_STATE_CHANGED = 70,
        SMART_EVENT_GO_EVENT_INFORM = 71,
        SMART_EVENT_ACTION_DONE = 72,
        SMART_EVENT_ON_SPELLCLICK = 73,
        SMART_EVENT_FRIENDLY_HEALTH_PCT = 74,
        SMART_EVENT_DISTANCE_CREATURE = 75,
        SMART_EVENT_DISTANCE_GAMEOBJECT = 76,
        SMART_EVENT_MAX,
    }

    public enum SmartAction
    {
        SMART_ACTION_NONE = 0,
        SMART_ACTION_TALK = 1,
        SMART_ACTION_SET_FACTION = 2,
        SMART_ACTION_MORPH_TO_ENTRY_OR_MODEL = 3,
        SMART_ACTION_SOUND = 4,
        SMART_ACTION_EMOTE = 5,
        SMART_ACTION_FAIL_QUEST = 6,
        SMART_ACTION_ADD_QUEST = 7,
        SMART_ACTION_SET_REACT_STATE = 8,
        SMART_ACTION_ACTIVATE_GOBJECT = 9,
        SMART_ACTION_RANDOM_EMOTE = 10,
        SMART_ACTION_CAST = 11,
        SMART_ACTION_SUMMON_CREATURE = 12,
        SMART_ACTION_THREAT_SINGLE_PCT = 13,
        SMART_ACTION_THREAT_ALL_PCT = 14,
        SMART_ACTION_CALL_AREAEXPLOREDOREVENTHAPPENS = 15,
        SMART_ACTION_UNUSED_16 = 16,
        SMART_ACTION_SET_EMOTE_STATE = 17,
        SMART_ACTION_SET_UNIT_FLAG = 18,
        SMART_ACTION_REMOVE_UNIT_FLAG = 19,
        SMART_ACTION_AUTO_ATTACK = 20,
        SMART_ACTION_COMBAT_MOVEMENT = 21,
        SMART_ACTION_SET_EVENT_PHASE = 22,
        SMART_ACTION_INC_EVENT_PHASE = 23,
        SMART_ACTION_EVADE = 24,
        SMART_ACTION_FLEE_FOR_ASSIST = 25,
        SMART_ACTION_CALL_GROUPEVENTHAPPENS = 26,
        SMART_ACTION_CALL_CASTEDCREATUREORGO = 27,
        SMART_ACTION_REMOVEAURASFROMSPELL = 28,
        SMART_ACTION_FOLLOW = 29,
        SMART_ACTION_RANDOM_PHASE = 30,
        SMART_ACTION_RANDOM_PHASE_RANGE = 31,
        SMART_ACTION_RESET_GOBJECT = 32,
        SMART_ACTION_KILLED_MONSTER = 33,
        SMART_ACTION_SET_INST_DATA = 34,
        SMART_ACTION_SET_INST_DATA64 = 35,
        SMART_ACTION_UPDATE_TEMPLATE = 36,
        SMART_ACTION_DIE = 37,
        SMART_ACTION_SET_IN_COMBAT_WITH_ZONE = 38,
        SMART_ACTION_CALL_FOR_HELP = 39,
        SMART_ACTION_SET_SHEATH = 40,
        SMART_ACTION_FORCE_DESPAWN = 41,
        SMART_ACTION_SET_INVINCIBILITY_HP_LEVEL = 42,
        SMART_ACTION_MOUNT_TO_ENTRY_OR_MODEL = 43,
        SMART_ACTION_SET_PHASE_MASK = 44,
        SMART_ACTION_SET_DATA = 45,
        SMART_ACTION_MOVE_FORWARD = 46,
        SMART_ACTION_SET_VISIBILITY = 47,
        SMART_ACTION_SET_ACTIVE = 48,
        SMART_ACTION_ATTACK_START = 49,
        SMART_ACTION_SUMMON_GO = 50,
        SMART_ACTION_KILL_UNIT = 51,
        SMART_ACTION_ACTIVATE_TAXI = 52,
        SMART_ACTION_WP_START = 53,
        SMART_ACTION_WP_PAUSE = 54,
        SMART_ACTION_WP_STOP = 55,
        SMART_ACTION_ADD_ITEM = 56,
        SMART_ACTION_REMOVE_ITEM = 57,
        SMART_ACTION_INSTALL_AI_TEMPLATE = 58,
        SMART_ACTION_SET_RUN = 59,
        SMART_ACTION_SET_FLY = 60,
        SMART_ACTION_SET_SWIM = 61,
        SMART_ACTION_TELEPORT = 62,
        SMART_ACTION_STORE_VARIABLE_DECIMAL = 63,
        SMART_ACTION_STORE_TARGET_LIST = 64,
        SMART_ACTION_WP_RESUME = 65,
        SMART_ACTION_SET_ORIENTATION = 66,
        SMART_ACTION_CREATE_TIMED_EVENT = 67,
        SMART_ACTION_PLAYMOVIE = 68,
        SMART_ACTION_MOVE_TO_POS = 69,
        SMART_ACTION_RESPAWN_TARGET = 70,
        SMART_ACTION_EQUIP = 71,
        SMART_ACTION_CLOSE_GOSSIP = 72,
        SMART_ACTION_TRIGGER_TIMED_EVENT = 73,
        SMART_ACTION_REMOVE_TIMED_EVENT = 74,
        SMART_ACTION_ADD_AURA = 75,
        SMART_ACTION_OVERRIDE_SCRIPT_BASE_OBJECT = 76,
        SMART_ACTION_RESET_SCRIPT_BASE_OBJECT = 77,
        SMART_ACTION_CALL_SCRIPT_RESET = 78,
        SMART_ACTION_SET_RANGED_MOVEMENT = 79,
        SMART_ACTION_CALL_TIMED_ACTIONLIST = 80,
        SMART_ACTION_SET_NPC_FLAG = 81,
        SMART_ACTION_ADD_NPC_FLAG = 82,
        SMART_ACTION_REMOVE_NPC_FLAG = 83,
        SMART_ACTION_SIMPLE_TALK = 84,
        SMART_ACTION_INVOKER_CAST = 85,
        SMART_ACTION_CROSS_CAST = 86,
        SMART_ACTION_CALL_RANDOM_TIMED_ACTIONLIST = 87,
        SMART_ACTION_CALL_RANDOM_RANGE_TIMED_ACTIONLIST = 88,
        SMART_ACTION_RANDOM_MOVE = 89,
        SMART_ACTION_SET_UNIT_FIELD_BYTES_1 = 90,
        SMART_ACTION_REMOVE_UNIT_FIELD_BYTES_1 = 91,
        SMART_ACTION_INTERRUPT_SPELL = 92,
        SMART_ACTION_SEND_GO_CUSTOM_ANIM = 93,
        SMART_ACTION_SET_DYNAMIC_FLAG = 94,
        SMART_ACTION_ADD_DYNAMIC_FLAG = 95,
        SMART_ACTION_REMOVE_DYNAMIC_FLAG = 96,
        SMART_ACTION_JUMP_TO_POS = 97,
        SMART_ACTION_SEND_GOSSIP_MENU = 98,
        SMART_ACTION_GO_SET_LOOT_STATE = 99,
        SMART_ACTION_SEND_TARGET_TO_TARGET = 100,
        SMART_ACTION_SET_HOME_POS = 101,
        SMART_ACTION_SET_HEALTH_REGEN = 102,
        SMART_ACTION_SET_ROOT = 103,
        SMART_ACTION_SET_GO_FLAG = 104,
        SMART_ACTION_ADD_GO_FLAG = 105,
        SMART_ACTION_REMOVE_GO_FLAG = 106,
        SMART_ACTION_SUMMON_CREATURE_GROUP = 107,
        SMART_ACTION_SET_POWER = 108,
        SMART_ACTION_ADD_POWER = 109,
        SMART_ACTION_REMOVE_POWER = 110,
        SMART_ACTION_MAX,
    }

    public enum SmartTarget
    {
        SMART_TARGET_NONE = 0,
        SMART_TARGET_SELF = 1,
        SMART_TARGET_VICTIM = 2,
        SMART_TARGET_HOSTILE_SECOND_AGGRO = 3,
        SMART_TARGET_HOSTILE_LAST_AGGRO = 4,
        SMART_TARGET_HOSTILE_RANDOM = 5,
        SMART_TARGET_HOSTILE_RANDOM_NOT_TOP = 6,
        SMART_TARGET_ACTION_INVOKER = 7,
        SMART_TARGET_POSITION = 8,
        SMART_TARGET_CREATURE_RANGE = 9,
        SMART_TARGET_CREATURE_GUID = 10,
        SMART_TARGET_CREATURE_DISTANCE = 11,
        SMART_TARGET_STORED = 12,
        SMART_TARGET_GAMEOBJECT_RANGE = 13,
        SMART_TARGET_GAMEOBJECT_GUID = 14,
        SMART_TARGET_GAMEOBJECT_DISTANCE = 15,
        SMART_TARGET_INVOKER_PARTY = 16,
        SMART_TARGET_PLAYER_RANGE = 17,
        SMART_TARGET_PLAYER_DISTANCE = 18,
        SMART_TARGET_CLOSEST_CREATURE = 19,
        SMART_TARGET_CLOSEST_GAMEOBJECT = 20,
        SMART_TARGET_CLOSEST_PLAYER = 21,
        SMART_TARGET_ACTION_INVOKER_VEHICLE = 22,
        SMART_TARGET_OWNER_OR_SUMMONER = 23,
        SMART_TARGET_THREAT_LIST = 24,
        SMART_TARGET_CLOSEST_ENEMY = 25,
        SMART_TARGET_CLOSEST_FRIENDLY = 26,
        SMART_TARGET_MAX,
    }

    public enum SmartReactState
    {
        REACT_PASSIVE = 0,
        REACT_DEFENSIVE = 1,
        REACT_AGGRESSIVE = 2
    }

    public enum SmartSourceType
    {
        SMART_SCRIPT_TYPE_CREATURE = 0,
        SMART_SCRIPT_TYPE_GAMEOBJECT = 1,
        SMART_SCRIPT_TYPE_AREATRIGGER = 2,
        SMART_SCRIPT_TYPE_EVENT_NYI = 3,
        SMART_SCRIPT_TYPE_GOSSIP_NYI = 4,
        SMART_SCRIPT_TYPE_QUEST_NYI = 5,
        SMART_SCRIPT_TYPE_SPELL_NYI = 6,
        SMART_SCRIPT_TYPE_TRANSPORT_NYI = 7,
        SMART_SCRIPT_TYPE_INSTANCE_NYI = 8,
        SMART_SCRIPT_TYPE_TIMED_ACTIONLIST = 9
    }

    public enum SmartSummonType
    {
        TEMPSUMMON_TIMED_OR_DEAD_DESPAWN = 1,
        TEMPSUMMON_TIMED_OR_CORPSE_DESPAWN = 2,
        TEMPSUMMON_TIMED_DESPAWN = 3,
        TEMPSUMMON_TIMED_DESPAWN_OUT_OF_COMBAT = 4,
        TEMPSUMMON_CORPSE_DESPAWN = 5,
        TEMPSUMMON_CORPSE_TIMED_DESPAWN = 6,
        TEMPSUMMON_DEAD_DESPAWN = 7,
        TEMPSUMMON_MANUAL_DESPAWN = 8
    }

    [Flags]
    public enum SmartCastFlags
    {
        SMARTCAST_NONE = 0x00,
        SMARTCAST_INTERRUPT_PREVIOUS = 0x01,
        SMARTCAST_TRIGGERED = 0x02,
        SMARTCAST_AURA_NOT_PRESENT = 0x20,
        SMARTCAST_COMBAT_MOVE = 0x40,
    }

    public enum ReactState
    {
        REACT_PASSIVE = 0,
        REACT_DEFENSIVE = 1,
        REACT_AGGRESSIVE = 2,
    }

    public enum SmartRespawnCondition
    {
        RESPAWN_CONDITION_NONE = 0,
        RESPAWN_CONDITION_MAP = 1,
        RESPAWN_CONDITION_AREA = 2,
    }

    public enum GoLootState
    {
        GAMEOBJECT_NOT_READY = 0,
        GAMEOBJECT_READY = 1,
        GAMEOBJECT_ACTIVATED = 2,
        GAMEOBJECT_JUST_DEACTIVATED = 3,
    }

    public enum SmartAiTemplates
    {
        SMARTAI_TEMPLATE_BASIC = 0,
        SMARTAI_TEMPLATE_CASTER = 1,
        SMARTAI_TEMPLATE_TURRET = 2,
        SMARTAI_TEMPLATE_PASSIVE = 3,
        SMARTAI_TEMPLATE_CAGED_GO_PART = 4,
        SMARTAI_TEMPLATE_CAGED_NPC_PART = 5,
    }

    [Flags]
    public enum GoFlags : uint
    {
        GO_FLAG_NONE                    = 0x00000000,
        GO_FLAG_IN_USE                  = 0x00000001,
        GO_FLAG_LOCKED                  = 0x00000002,
        GO_FLAG_INTERACT_COND           = 0x00000004,
        GO_FLAG_TRANSPORT               = 0x00000008,
        GO_FLAG_NOT_SELECTABLE          = 0x00000010,
        GO_FLAG_NODESPAWN               = 0x00000020,
        GO_FLAG_TRIGGERED               = 0x00000040,
        GO_FLAG_DAMAGED                 = 0x00000200,
        GO_FLAG_DESTROYED               = 0x00000400,
    }

    [Flags]
    public enum DynamicFlags : uint
    {
        UNIT_DYNFLAG_NONE                       = 0x0000,
        UNIT_DYNFLAG_LOOTABLE                   = 0x0001,
        UNIT_DYNFLAG_TRACK_UNIT                 = 0x0002,
        UNIT_DYNFLAG_TAPPED                     = 0x0004,       // Lua_UnitIsTapped
        UNIT_DYNFLAG_TAPPED_BY_PLAYER           = 0x0008,       // Lua_UnitIsTappedByPlayer
        UNIT_DYNFLAG_SPECIALINFO                = 0x0010,
        UNIT_DYNFLAG_DEAD                       = 0x0020,
        UNIT_DYNFLAG_REFER_A_FRIEND             = 0x0040,
        UNIT_DYNFLAG_TAPPED_BY_ALL_THREAT_LIST  = 0x0080        // Lua_UnitIsTappedByAllThreatList
    }

    public enum UnitStandStateType : uint
    {
        UNIT_STAND_STATE_STAND             = 0,
        UNIT_STAND_STATE_SIT               = 1,
        UNIT_STAND_STATE_SIT_CHAIR         = 2,
        UNIT_STAND_STATE_SLEEP             = 3,
        UNIT_STAND_STATE_SIT_LOW_CHAIR     = 4,
        UNIT_STAND_STATE_SIT_MEDIUM_CHAIR  = 5,
        UNIT_STAND_STATE_SIT_HIGH_CHAIR    = 6,
        UNIT_STAND_STATE_DEAD              = 7,
        UNIT_STAND_STATE_KNEEL             = 8,
        UNIT_STAND_STATE_SUBMERGED         = 9
    }

    [Flags]
    public enum UnitStandFlags : uint
    {
        UNIT_STAND_FLAGS_UNK1         = 0x01,
        UNIT_STAND_FLAGS_CREEP        = 0x02,
        UNIT_STAND_FLAGS_UNTRACKABLE  = 0x04,
        UNIT_STAND_FLAGS_UNK4         = 0x08,
        UNIT_STAND_FLAGS_UNK5         = 0x10,
        UNIT_STAND_FLAGS_ALL          = 0xFF
    }

    [Flags]
    public enum UnitBytes1_Flags : uint
    {
        UNIT_BYTE1_FLAG_ALWAYS_STAND    = 0x01,
        UNIT_BYTE1_FLAG_HOVER           = 0x02,
        UNIT_BYTE1_FLAG_UNK_3           = 0x04,
        UNIT_BYTE1_FLAG_ALL             = 0xFF
    }

    [Flags]
    public enum SmartEventFlags : uint
    {
        EVENT_FLAG_NONE               = 0x00,
        EVENT_FLAG_NOT_REPEATABLE     = 0x01,
        EVENT_FLAG_NORMAL_DUNGEON     = 0x02,
        EVENT_FLAG_HEROIC_DUNGEON     = 0x04,
        EVENT_FLAG_NORMAL_RAID        = 0x08,
        EVENT_FLAG_HEROIC_RAID        = 0x10,
        EVENT_FLAG_DEBUG_ONLY         = 0x80,
    }

    [Flags]
    public enum UnitFlags : uint
    {
        UNIT_FLAG_NONE                  = 0x00000000,
        UNIT_FLAG_SERVER_CONTROLLED     = 0x00000001,           // set only when unit movement is controlled by server - by SPLINE/MONSTER_MOVE packets, together with UNIT_FLAG_STUNNED; only set to units controlled by client; client function CGUnit_C::IsClientControlled returns false when set for owner
        UNIT_FLAG_NON_ATTACKABLE        = 0x00000002,           // not attackable
        UNIT_FLAG_DISABLE_MOVE          = 0x00000004,
        UNIT_FLAG_PVP_ATTACKABLE        = 0x00000008,           // allow apply pvp rules to attackable state in addition to faction dependent state
        UNIT_FLAG_RENAME                = 0x00000010,
        UNIT_FLAG_PREPARATION           = 0x00000020,           // don't take reagents for spells with SPELL_ATTR5_NO_REAGENT_WHILE_PREP
        UNIT_FLAG_UNK_6                 = 0x00000040,
        UNIT_FLAG_NOT_ATTACKABLE_1      = 0x00000080,           // ?? (UNIT_FLAG_PVP_ATTACKABLE | UNIT_FLAG_NOT_ATTACKABLE_1) is NON_PVP_ATTACKABLE
        UNIT_FLAG_IMMUNE_TO_PC          = 0x00000100,           // disables combat/assistance with PlayerCharacters (PC) - see Unit::_IsValidAttackTarget, Unit::_IsValidAssistTarget
        UNIT_FLAG_IMMUNE_TO_NPC         = 0x00000200,           // disables combat/assistance with NonPlayerCharacters (NPC) - see Unit::_IsValidAttackTarget, Unit::_IsValidAssistTarget
        UNIT_FLAG_LOOTING               = 0x00000400,           // loot animation
        UNIT_FLAG_PET_IN_COMBAT         = 0x00000800,           // in combat?, 2.0.8
        UNIT_FLAG_PVP                   = 0x00001000,           // changed in 3.0.3
        UNIT_FLAG_SILENCED              = 0x00002000,           // silenced, 2.1.1
        UNIT_FLAG_UNK_14                = 0x00004000,           // 2.0.8
        UNIT_FLAG_UNK_15                = 0x00008000,
        UNIT_FLAG_UNK_16                = 0x00010000,
        UNIT_FLAG_PACIFIED              = 0x00020000,           // 3.0.3 ok
        UNIT_FLAG_STUNNED               = 0x00040000,           // 3.0.3 ok
        UNIT_FLAG_IN_COMBAT             = 0x00080000,
        UNIT_FLAG_TAXI_FLIGHT           = 0x00100000,           // disable casting at client side spell not allowed by taxi flight (mounted?), probably used with 0x4 flag
        UNIT_FLAG_DISARMED              = 0x00200000,           // 3.0.3, disable melee spells casting..., "Required melee weapon" added to melee spells tooltip.
        UNIT_FLAG_CONFUSED              = 0x00400000,
        UNIT_FLAG_FLEEING               = 0x00800000,
        UNIT_FLAG_PLAYER_CONTROLLED     = 0x01000000,           // used in spell Eyes of the Beast for pet... let attack by controlled creature
        UNIT_FLAG_NOT_SELECTABLE        = 0x02000000,
        UNIT_FLAG_SKINNABLE             = 0x04000000,
        UNIT_FLAG_MOUNT                 = 0x08000000,
        UNIT_FLAG_UNK_28                = 0x10000000,
        UNIT_FLAG_UNK_29                = 0x20000000,           // used in Feing Death spell
        UNIT_FLAG_SHEATHE               = 0x40000000,
        UNIT_FLAG_UNK_31                = 0x80000000,
    }

    [Flags]
    public enum UnitFlags2 : uint
    {
        UNIT_FLAG2_NONE                         = 0x00000000,
        UNIT_FLAG2_FEIGN_DEATH                  = 0x00000001,
        UNIT_FLAG2_UNK1                         = 0x00000002,   // Hide unit model (show only player equip)
        UNIT_FLAG2_IGNORE_REPUTATION            = 0x00000004,
        UNIT_FLAG2_COMPREHEND_LANG              = 0x00000008,
        UNIT_FLAG2_MIRROR_IMAGE                 = 0x00000010,
        UNIT_FLAG2_INSTANTLY_APPEAR_MODEL       = 0x00000020,   // Unit model instantly appears when summoned (does not fade in)
        UNIT_FLAG2_FORCE_MOVEMENT               = 0x00000040,
        UNIT_FLAG2_DISARM_OFFHAND               = 0x00000080,
        UNIT_FLAG2_DISABLE_PRED_STATS           = 0x00000100,   // Player has disabled predicted stats (Used by raid frames)
        UNIT_FLAG2_DISARM_RANGED                = 0x00000400,   // this does not disable ranged weapon display (maybe additional flag needed?)
        UNIT_FLAG2_REGENERATE_POWER             = 0x00000800,
        UNIT_FLAG2_RESTRICT_PARTY_INTERACTION   = 0x00001000,   // Restrict interaction to party or raid
        UNIT_FLAG2_PREVENT_SPELL_CLICK          = 0x00002000,   // Prevent spellclick
        UNIT_FLAG2_ALLOW_ENEMY_INTERACT         = 0x00004000,
        UNIT_FLAG2_DISABLE_TURN                 = 0x00008000,
        UNIT_FLAG2_UNK2                         = 0x00010000,
        UNIT_FLAG2_PLAY_DEATH_ANIM              = 0x00020000,   // Plays special death animation upon death
        UNIT_FLAG2_ALLOW_CHEAT_SPELLS           = 0x00040000    // Allows casting spells with AttributesEx7 & SPELL_ATTR7_IS_CHEAT_SPELL
    }

    [Flags]
    public enum NpcFlags : uint
    {
        UNIT_NPC_FLAG_NONE                  = 0x00000000,
        UNIT_NPC_FLAG_GOSSIP                = 0x00000001,       // 100%
        UNIT_NPC_FLAG_QUESTGIVER            = 0x00000002,       // guessed, probably ok
        UNIT_NPC_FLAG_UNK1                  = 0x00000004,
        UNIT_NPC_FLAG_UNK2                  = 0x00000008,
        UNIT_NPC_FLAG_TRAINER               = 0x00000010,       // 100%
        UNIT_NPC_FLAG_TRAINER_CLASS         = 0x00000020,       // 100%
        UNIT_NPC_FLAG_TRAINER_PROFESSION    = 0x00000040,       // 100%
        UNIT_NPC_FLAG_VENDOR                = 0x00000080,       // 100%
        UNIT_NPC_FLAG_VENDOR_AMMO           = 0x00000100,       // 100%, general goods vendor
        UNIT_NPC_FLAG_VENDOR_FOOD           = 0x00000200,       // 100%
        UNIT_NPC_FLAG_VENDOR_POISON         = 0x00000400,       // guessed
        UNIT_NPC_FLAG_VENDOR_REAGENT        = 0x00000800,       // 100%
        UNIT_NPC_FLAG_REPAIR                = 0x00001000,       // 100%
        UNIT_NPC_FLAG_FLIGHTMASTER          = 0x00002000,       // 100%
        UNIT_NPC_FLAG_SPIRITHEALER          = 0x00004000,       // guessed
        UNIT_NPC_FLAG_SPIRITGUIDE           = 0x00008000,       // guessed
        UNIT_NPC_FLAG_INNKEEPER             = 0x00010000,       // 100%
        UNIT_NPC_FLAG_BANKER                = 0x00020000,       // 100%
        UNIT_NPC_FLAG_PETITIONER            = 0x00040000,       // 100% 0xC0000 = guild petitions, 0x40000 = arena team petitions
        UNIT_NPC_FLAG_TABARDDESIGNER        = 0x00080000,       // 100%
        UNIT_NPC_FLAG_BATTLEMASTER          = 0x00100000,       // 100%
        UNIT_NPC_FLAG_AUCTIONEER            = 0x00200000,       // 100%
        UNIT_NPC_FLAG_STABLEMASTER          = 0x00400000,       // 100%
        UNIT_NPC_FLAG_GUILD_BANKER          = 0x00800000,       // cause client to send 997 opcode
        UNIT_NPC_FLAG_SPELLCLICK            = 0x01000000,       // cause client to send 1015 opcode (spell click)
        UNIT_NPC_FLAG_PLAYER_VEHICLE        = 0x02000000        // players with mounts that have vehicle data should have it set
    }

    [Flags]
    public enum SmartPhaseMasks
    {
        SMART_EVENT_PHASE_ALWAYS            = 0x00,
        SMART_EVENT_PHASE_1                 = 0x01,
        SMART_EVENT_PHASE_2                 = 0x02,
        SMART_EVENT_PHASE_3                 = 0x04,
        SMART_EVENT_PHASE_4                 = 0x08,
        SMART_EVENT_PHASE_5                 = 0x10,
        SMART_EVENT_PHASE_6                 = 0x20,
    }

    [Flags]
    public enum PhaseMasks
    {
        PHASEMASK_NEVER    = 0x00,
        PHASEMASK_2        = 0x02,
        PHASEMASK_3        = 0x04,
        PHASEMASK_4        = 0x08,
        PHASEMASK_5        = 0x10,
        PHASEMASK_6        = 0x20,
        PHASEMASK_7        = 0x40,
        PHASEMASK_8        = 0x80,
        PHASEMASK_9        = 0x100,
        PHASEMASK_10       = 0x200,
        PHASEMASK_11       = 0x400,
        PHASEMASK_12       = 0x800,
        PHASEMASK_13       = 0x1000,
        PHASEMASK_14       = 0x2000,
        PHASEMASK_15       = 0x4000,
        // etc....
        PHASEMASK_ANYWHERE = ~0,   //! -1, 0xFFFFFFFF, etc.
    }

    public enum PowerTypes : int
    {
        POWER_MANA                          = 0,
        POWER_RAGE                          = 1,
        POWER_FOCUS                         = 2,
        POWER_ENERGY                        = 3,
        POWER_HAPPINESS                     = 4,
        POWER_RUNE                          = 5,
        POWER_RUNIC_POWER                   = 6,
        //MAX_POWERS                          = 7,
        //POWER_ALL                           = 127,    // default for class?
        //POWER_HEALTH                        = 0xFFFFFFFE    // (-2 as signed value)
        POWER_HEALTH = -2,
    }

    public enum GoStates
    {
        GO_STATE_NOT_READY                  = 0,
        GO_STATE_READY                      = 1,
        GO_STATE_ACTIVATED                  = 2,
        GO_STATE_DEACTIVATED                = 3,
    }

    public enum MovementGeneratorType
    {
        IDLE_MOTION_TYPE      = 0,                              // IdleMovementGenerator.h
        RANDOM_MOTION_TYPE    = 1,                              // RandomMovementGenerator.h
        WAYPOINT_MOTION_TYPE  = 2,                              // WaypointMovementGenerator.h
        MAX_DB_MOTION_TYPE    = 3,                              // *** this and below motion types can't be set in DB.
        ANIMAL_RANDOM_MOTION_TYPE = MAX_DB_MOTION_TYPE,         // AnimalRandomMovementGenerator.h
        CONFUSED_MOTION_TYPE  = 4,                              // ConfusedMovementGenerator.h
        CHASE_MOTION_TYPE     = 5,                              // TargetedMovementGenerator.h
        HOME_MOTION_TYPE      = 6,                              // HomeMovementGenerator.h
        FLIGHT_MOTION_TYPE    = 7,                              // WaypointMovementGenerator.h
        POINT_MOTION_TYPE     = 8,                              // PointMovementGenerator.h
        FLEEING_MOTION_TYPE   = 9,                              // FleeingMovementGenerator.h
        DISTRACT_MOTION_TYPE  = 10,                             // IdleMovementGenerator.h
        ASSISTANCE_MOTION_TYPE= 11,                             // PointMovementGenerator.h (first part of flee for assistance)
        ASSISTANCE_DISTRACT_MOTION_TYPE = 12,                   // IdleMovementGenerator.h (second part of flee for assistance)
        TIMED_FLEEING_MOTION_TYPE = 13,                         // FleeingMovementGenerator.h (alt.second part of flee for assistance)
        FOLLOW_MOTION_TYPE    = 14,
        ROTATE_MOTION_TYPE    = 15,
        EFFECT_MOTION_TYPE    = 16,
    }

    public enum SpellSchools
    {
        SPELL_SCHOOL_NORMAL                 = 0,
        SPELL_SCHOOL_HOLY                   = 1,
        SPELL_SCHOOL_FIRE                   = 2,
        SPELL_SCHOOL_NATURE                 = 3,
        SPELL_SCHOOL_FROST                  = 4,
        SPELL_SCHOOL_SHADOW                 = 5,
        SPELL_SCHOOL_ARCANE                 = 6
    }

    public enum SheathState
    {
        SHEATH_STATE_UNARMED  = 0,                              // non prepared weapon
        SHEATH_STATE_MELEE    = 1,                              // prepared melee weapon
        SHEATH_STATE_RANGED   = 2                               // prepared ranged weapon
    }

    public enum TempSummonType
    {
        TEMPSUMMON_TIMED_OR_DEAD_DESPAWN       = 1,             // despawns after a specified time OR when the creature disappears
        TEMPSUMMON_TIMED_OR_CORPSE_DESPAWN     = 2,             // despawns after a specified time OR when the creature dies
        TEMPSUMMON_TIMED_DESPAWN               = 3,             // despawns after a specified time
        TEMPSUMMON_TIMED_DESPAWN_OUT_OF_COMBAT = 4,             // despawns after a specified time after the creature is out of combat
        TEMPSUMMON_CORPSE_DESPAWN              = 5,             // despawns instantly after death
        TEMPSUMMON_CORPSE_TIMED_DESPAWN        = 6,             // despawns after a specified time after death
        TEMPSUMMON_DEAD_DESPAWN                = 7,             // despawns when the creature disappears
        TEMPSUMMON_MANUAL_DESPAWN              = 8              // despawns when UnSummon() is called
    }

    public enum UnitFieldBytes1Types
    {
        UNIT_STAND_STAND_STATE_TYPE = 0,
        UNIT_PET_TALENTS_TYPE       = 1,
        UNIT_STAND_FLAGS_TYPE       = 2,
        UNIT_BYTES1_FLAGS_TYPE      = 3,
    }

    public enum SmartActionlistTimerUpdateType
    {
        ACTIONLIST_UPDATE_OUT_OF_COMBAT = 0,
        ACTIONLIST_UPDATE_IN_COMBAT     = 1,
        ACTIONLIST_UPDATE_ALWAYS        = 2,
    }

    public enum ConditionSourceTypes
    {
        CONDITION_SOURCE_TYPE_NONE                           = 0,
        CONDITION_SOURCE_TYPE_CREATURE_LOOT_TEMPLATE         = 1,
        CONDITION_SOURCE_TYPE_DISENCHANT_LOOT_TEMPLATE       = 2,
        CONDITION_SOURCE_TYPE_FISHING_LOOT_TEMPLATE          = 3,
        CONDITION_SOURCE_TYPE_GAMEOBJECT_LOOT_TEMPLATE       = 4,
        CONDITION_SOURCE_TYPE_ITEM_LOOT_TEMPLATE             = 5,
        CONDITION_SOURCE_TYPE_MAIL_LOOT_TEMPLATE             = 6,
        CONDITION_SOURCE_TYPE_MILLING_LOOT_TEMPLATE          = 7,
        CONDITION_SOURCE_TYPE_PICKPOCKETING_LOOT_TEMPLATE    = 8,
        CONDITION_SOURCE_TYPE_PROSPECTING_LOOT_TEMPLATE      = 9,
        CONDITION_SOURCE_TYPE_REFERENCE_LOOT_TEMPLATE        = 10,
        CONDITION_SOURCE_TYPE_SKINNING_LOOT_TEMPLATE         = 11,
        CONDITION_SOURCE_TYPE_SPELL_LOOT_TEMPLATE            = 12,
        CONDITION_SOURCE_TYPE_SPELL_IMPLICIT_TARGET          = 13,
        CONDITION_SOURCE_TYPE_GOSSIP_MENU                    = 14,
        CONDITION_SOURCE_TYPE_GOSSIP_MENU_OPTION             = 15,
        CONDITION_SOURCE_TYPE_CREATURE_TEMPLATE_VEHICLE      = 16,
        CONDITION_SOURCE_TYPE_SPELL                          = 17,
        CONDITION_SOURCE_TYPE_SPELL_CLICK_EVENT              = 18,
        CONDITION_SOURCE_TYPE_QUEST_ACCEPT                   = 19,
        CONDITION_SOURCE_TYPE_QUEST_SHOW_MARK                = 20,
        CONDITION_SOURCE_TYPE_VEHICLE_SPELL                  = 21,
        CONDITION_SOURCE_TYPE_SMART_EVENT                    = 22,
        CONDITION_SOURCE_TYPE_NPC_VENDOR                     = 23,
        CONDITION_SOURCE_TYPE_SPELL_PROC                     = 24,
        //CONDITION_SOURCE_TYPE_PHASE_DEFINITION               = 25,
        CONDITION_SOURCE_TYPE_MAX,
    }

    public enum ConditionTypes
    {                                                           // value1           value2         value3
        CONDITION_NONE                  = 0,                    // 0                0              0                  always true
        CONDITION_AURA                  = 1,                    // spell_id         effindex       use target?        true if player (or target, if value3) has aura of spell_id with effect effindex
        CONDITION_ITEM                  = 2,                    // item_id          count          bank               true if has #count of item_ids (if 'bank' is set it searches in bank slots too)
        CONDITION_ITEM_EQUIPPED         = 3,                    // item_id          0              0                  true if has item_id equipped
        CONDITION_ZONEID                = 4,                    // zone_id          0              0                  true if in zone_id
        CONDITION_REPUTATION_RANK       = 5,                    // faction_id       rankMask       0                  true if has min_rank for faction_id
        CONDITION_TEAM                  = 6,                    // player_team      0,             0                  469 - Alliance, 67 - Horde)
        CONDITION_SKILL                 = 7,                    // skill_id         skill_value    0                  true if has skill_value for skill_id
        CONDITION_QUESTREWARDED         = 8,                    // quest_id         0              0                  true if quest_id was rewarded before
        CONDITION_QUESTTAKEN            = 9,                    // quest_id         0,             0                  true while quest active
        CONDITION_DRUNKENSTATE          = 10,                   // DrunkenState     0,             0                  true if player is drunk enough
        CONDITION_WORLD_STATE           = 11,                   // index            value          0                  true if world has the value for the index
        CONDITION_ACTIVE_EVENT          = 12,                   // event_id         0              0                  true if event is active
        CONDITION_INSTANCE_INFO         = 13,                   // entry            data           type               true if the instance info defined by type (enum InstanceInfo) equals data.
        CONDITION_QUEST_NONE            = 14,                   // quest_id         0              0                  true if doesn't have quest saved
        CONDITION_CLASS                 = 15,                   // class            0              0                  true if player's class is equal to class
        CONDITION_RACE                  = 16,                   // race             0              0                  true if player's race is equal to race
        CONDITION_ACHIEVEMENT           = 17,                   // achievement_id   0              0                  true if achievement is complete
        CONDITION_TITLE                 = 18,                   // title id         0              0                  true if player has title
        CONDITION_SPAWNMASK             = 19,                   // spawnMask        0              0                  true if in spawnMask
        CONDITION_GENDER                = 20,                   // gender           0              0                  true if player's gender is equal to gender
        CONDITION_UNIT_STATE            = 21,                   // unitState        0              0                  true if unit has unitState
        CONDITION_MAPID                 = 22,                   // map_id           0              0                  true if in map_id
        CONDITION_AREAID                = 23,                   // area_id          0              0                  true if in area_id
        CONDITION_CREATURE_TYPE         = 24,                   // cinfo.type       0              0                  true if creature_template.type = value1
        CONDITION_SPELL                 = 25,                   // spell_id         0              0                  true if player has learned spell
        CONDITION_PHASEMASK             = 26,                   // phasemask        0              0                  true if object is in phasemask
        CONDITION_LEVEL                 = 27,                   // level            ComparisonType 0                  true if unit's level is equal to param1 (param2 can modify the statement)
        CONDITION_QUEST_COMPLETE        = 28,                   // quest_id         0              0                  true if player has quest_id with all objectives complete, but not yet rewarded
        CONDITION_NEAR_CREATURE         = 29,                   // creature entry   distance       0                  true if there is a creature of entry in range
        CONDITION_NEAR_GAMEOBJECT       = 30,                   // gameobject entry distance       0                  true if there is a gameobject of entry in range
        CONDITION_OBJECT_ENTRY          = 31,                   // TypeID           entry          0                  true if object is type TypeID and the entry is 0 or matches entry of the object
        CONDITION_TYPE_MASK             = 32,                   // TypeMask         0              0                  true if object is type object's TypeMask matches provided TypeMask
        CONDITION_RELATION_TO           = 33,                   // ConditionTarget  RelationType   0                  true if object is in given relation with object specified by ConditionTarget
        CONDITION_REACTION_TO           = 34,                   // ConditionTarget  rankMask       0                  true if object's reaction matches rankMask object specified by ConditionTarget
        CONDITION_DISTANCE_TO           = 35,                   // ConditionTarget  distance       ComparisonType     true if object and ConditionTarget are within distance given by parameters
        CONDITION_ALIVE                 = 36,                   // 0                0              0                  true if unit is alive
        CONDITION_HP_VAL                = 37,                   // hpVal            ComparisonType 0                  true if unit's hp matches given value
        CONDITION_HP_PCT                = 38,                   // hpPct            ComparisonType 0                  true if unit's hp matches given pct
        CONDITION_MAX                   = 39                    // MAX
    }

    public enum SpellEffIndex
    {
        EFFECT_0 = 0,
        EFFECT_1 = 1,
        EFFECT_2 = 2
    }

    //! ErrorType in Conditions
    public enum SpellCastResult
    {
        NONE = 0,
        AFFECTING_COMBAT = 1,
        ALREADY_AT_FULL_HEALTH = 2,
        ALREADY_AT_FULL_MANA = 3,
        ALREADY_AT_FULL_POWER = 4,
        ALREADY_BEING_TAMED = 5,
        ALREADY_HAVE_CHARM = 6,
        ALREADY_HAVE_SUMMON = 7,
        ALREADY_OPEN = 8,
        AURA_BOUNCED = 9,
        AUTOTRACK_INTERRUPTED = 10,
        BAD_IMPLICIT_TARGETS = 11,
        BAD_TARGETS = 12,
        CANT_BE_CHARMED = 13,
        CANT_BE_DISENCHANTED = 14,
        CANT_BE_DISENCHANTED_SKILL = 15,
        CANT_BE_MILLED = 16,
        CANT_BE_PROSPECTED = 17,
        CANT_CAST_ON_TAPPED = 18,
        CANT_DUEL_WHILE_INVISIBLE = 19,
        CANT_DUEL_WHILE_STEALTHED = 20,
        CANT_STEALTH = 21,
        CASTER_AURASTATE = 22,
        CASTER_DEAD = 23,
        CHARMED = 24,
        CHEST_IN_USE = 25,
        CONFUSED = 26,
        DONT_REPORT = 27,
        EQUIPPED_ITEM = 28,
        EQUIPPED_ITEM_CLASS = 29,
        EQUIPPED_ITEM_CLASS_MAINHAND = 30,
        EQUIPPED_ITEM_CLASS_OFFHAND = 31,
        ERROR = 32,
        FIZZLE = 33,
        FLEEING = 34,
        FOOD_LOWLEVEL = 35,
        HIGHLEVEL = 36,
        HUNGER_SATIATED = 37,
        IMMUNE = 38,
        INCORRECT_AREA = 39,
        INTERRUPTED = 40,
        INTERRUPTED_COMBAT = 41,
        ITEM_ALREADY_ENCHANTED = 42,
        ITEM_GONE = 43,
        ITEM_NOT_FOUND = 44,
        ITEM_NOT_READY = 45,
        LEVEL_REQUIREMENT = 46,
        LINE_OF_SIGHT = 47,
        LOWLEVEL = 48,
        LOW_CASTLEVEL = 49,
        MAINHAND_EMPTY = 50,
        MOVING = 51,
        NEED_AMMO = 52,
        NEED_AMMO_POUCH = 53,
        NEED_EXOTIC_AMMO = 54,
        NEED_MORE_ITEMS = 55,
        NOPATH = 56,
        NOT_BEHIND = 57,
        NOT_FISHABLE = 58,
        NOT_FLYING = 59,
        NOT_HERE = 60,
        NOT_INFRONT = 61,
        NOT_IN_CONTROL = 62,
        NOT_KNOWN = 63,
        NOT_MOUNTED = 64,
        NOT_ON_TAXI = 65,
        NOT_ON_TRANSPORT = 66,
        NOT_READY = 67,
        NOT_SHAPESHIFT = 68,
        NOT_STANDING = 69,
        NOT_TRADEABLE = 70,
        NOT_TRADING = 71,
        NOT_UNSHEATHED = 72,
        NOT_WHILE_GHOST = 73,
        NOT_WHILE_LOOTING = 74,
        NO_AMMO = 75,
        NO_CHARGES_REMAIN = 76,
        NO_CHAMPION = 77,
        NO_COMBO_POINTS = 78,
        NO_DUELING = 79,
        NO_ENDURANCE = 80,
        NO_FISH = 81,
        NO_ITEMS_WHILE_SHAPESHIFTED = 82,
        NO_MOUNTS_ALLOWED = 83,
        NO_PET = 84,
        NO_POWER = 85,
        NOTHING_TO_DISPEL = 86,
        NOTHING_TO_STEAL = 87,
        ONLY_ABOVEWATER = 88,
        ONLY_DAYTIME = 89,
        ONLY_INDOORS = 90,
        ONLY_MOUNTED = 91,
        ONLY_NIGHTTIME = 92,
        ONLY_OUTDOORS = 93,
        ONLY_SHAPESHIFT = 94,
        ONLY_STEALTHED = 95,
        ONLY_UNDERWATER = 96,
        OUT_OF_RANGE = 97,
        PACIFIED = 98,
        POSSESSED = 99,
        REAGENTS = 100,
        REQUIRES_AREA = 101,
        REQUIRES_SPELL_FOCUS = 102,
        ROOTED = 103,
        SILENCED = 104,
        SPELL_IN_PROGRESS = 105,
        SPELL_LEARNED = 106,
        SPELL_UNAVAILABLE = 107,
        STUNNED = 108,
        TARGETS_DEAD = 109,
        TARGET_AFFECTING_COMBAT = 110,
        TARGET_AURASTATE = 111,
        TARGET_DUELING = 112,
        TARGET_ENEMY = 113,
        TARGET_ENRAGED = 114,
        TARGET_FRIENDLY = 115,
        TARGET_IN_COMBAT = 116,
        TARGET_IS_PLAYER = 117,
        TARGET_IS_PLAYER_CONTROLLED = 118,
        TARGET_NOT_DEAD = 119,
        TARGET_NOT_IN_PARTY = 120,
        TARGET_NOT_LOOTED = 121,
        TARGET_NOT_PLAYER = 122,
        TARGET_NO_POCKETS = 123,
        TARGET_NO_WEAPONS = 124,
        TARGET_NO_RANGED_WEAPONS = 125,
        TARGET_UNSKINNABLE = 126,
        THIRST_SATIATED = 127,
        TOO_CLOSE = 128,
        TOO_MANY_OF_ITEM = 129,
        TOTEM_CATEGORY = 130,
        TOTEMS = 131,
        TRY_AGAIN = 132,
        UNIT_NOT_BEHIND = 133,
        UNIT_NOT_INFRONT = 134,
        WRONG_PET_FOOD = 135,
        NOT_WHILE_FATIGUED = 136,
        TARGET_NOT_IN_INSTANCE = 137,
        NOT_WHILE_TRADING = 138,
        TARGET_NOT_IN_RAID = 139,
        TARGET_FREEFORALL = 140,
        NO_EDIBLE_CORPSES = 141,
        ONLY_BATTLEGROUNDS = 142,
        TARGET_NOT_GHOST = 143,
        TRANSFORM_UNUSABLE = 144,
        WRONG_WEATHER = 145,
        DAMAGE_IMMUNE = 146,
        PREVENTED_BY_MECHANIC = 147,
        PLAY_TIME = 148,
        REPUTATION = 149,
        MIN_SKILL = 150,
        NOT_IN_ARENA = 151,
        NOT_ON_SHAPESHIFT = 152,
        NOT_ON_STEALTHED = 153,
        NOT_ON_DAMAGE_IMMUNE = 154,
        NOT_ON_MOUNTED = 155,
        TOO_SHALLOW = 156,
        TARGET_NOT_IN_SANCTUARY = 157,
        TARGET_IS_TRIVIAL = 158,
        BM_OR_INVISGOD = 159,
        EXPERT_RIDING_REQUIREMENT = 160,
        ARTISAN_RIDING_REQUIREMENT = 161,
        NOT_IDLE = 162,
        NOT_INACTIVE = 163,
        PARTIAL_PLAYTIME = 164,
        NO_PLAYTIME = 165,
        NOT_IN_BATTLEGROUND = 166,
        NOT_IN_RAID_INSTANCE = 167,
        ONLY_IN_ARENA = 168,
        TARGET_LOCKED_TO_RAID_INSTANCE = 169,
        ON_USE_ENCHANT = 170,
        NOT_ON_GROUND = 171,
        CUSTOM_ERROR = 172,
        CANT_DO_THAT_RIGHT_NOW = 173,
        TOO_MANY_SOCKETS = 174,
        INVALID_GLYPH = 175,
        UNIQUE_GLYPH = 176,
        GLYPH_SOCKET_LOCKED = 177,
        NO_VALID_TARGETS = 178,
        ITEM_AT_MAX_CHARGES = 179,
        NOT_IN_BARBERSHOP = 180,
        FISHING_TOO_LOW = 181,
        ITEM_ENCHANT_TRADE_WINDOW = 182,
        SUMMON_PENDING = 183,
        MAX_SOCKETS = 184,
        PET_CAN_RENAME = 185,
        TARGET_CANNOT_BE_RESURRECTED = 186,
    }

    //! ErrorTextId
    public enum SpellCustomErrors
    {
        ERROR_NONE                             =  0,
        ERROR_CUSTOM_MSG                       =  1, // Something bad happened, and we want to display a custom message!
        ERROR_ALEX_BROKE_QUEST                 =  2, // Alex broke your quest! Thank him later!
        ERROR_NEED_HELPLESS_VILLAGER           =  3, // This spell may only be used on Helpless Wintergarde Villagers that have not been rescued.
        ERROR_NEED_WARSONG_DISGUISE            =  4, // Requires that you be wearing the Warsong Orc Disguise.
        ERROR_REQUIRES_PLAGUE_WAGON            =  5, // You must be closer to a plague wagon in order to drop off your 7th Legion Siege Engineer.
        ERROR_CANT_TARGET_FRIENDLY_NONPARTY    =  6, // You cannot target friendly units outside your party.
        ERROR_NEED_CHILL_NYMPH                 =  7, // You must target a weakened chill nymph.
        ERROR_MUST_BE_IN_ENKILAH               =  8, // The Imbued Scourge Shroud will only work when equipped in the Temple City of En'kilah.
        ERROR_REQUIRES_CORPSE_DUST             =  9, // Requires Corpse Dust
        ERROR_CANT_SUMMON_GARGOYLE             = 10, // You cannot summon another gargoyle yet.
        ERROR_NEED_CORPSE_DUST_IF_NO_TARGET    = 11, // Requires Corpse Dust if the target is not dead and humanoid.
        ERROR_MUST_BE_AT_SHATTERHORN           = 12, // Can only be placed near Shatterhorn
        ERROR_MUST_TARGET_PROTO_DRAKE_EGG      = 13, // You must first select a Proto-Drake Egg.
        ERROR_MUST_BE_CLOSE_TO_TREE            = 14, // You must be close to a marked tree.
        ERROR_MUST_TARGET_TURKEY               = 15, // You must target a Fjord Turkey.
        ERROR_MUST_TARGET_HAWK                 = 16, // You must target a Fjord Hawk.
        ERROR_TOO_FAR_FROM_BOUY                = 17, // You are too far from the bouy.
        ERROR_MUST_BE_CLOSE_TO_OIL_SLICK       = 18, // Must be used near an oil slick.
        ERROR_MUST_BE_CLOSE_TO_BOUY            = 19, // You must be closer to the buoy!
        ERROR_WYRMREST_VANQUISHER              = 20, // You may only call for the aid of a Wyrmrest Vanquisher in Wyrmrest Temple, The Dragon Wastes, Galakrond's Rest or The Wicked Coil.
        ERROR_MUST_TARGET_ICE_HEART_JORMUNGAR  = 21, // That can only be used on a Ice Heart Jormungar Spawn.
        ERROR_MUST_BE_CLOSE_TO_SINKHOLE        = 22, // You must be closer to a sinkhole to use your map.
        ERROR_REQUIRES_HAROLD_LANE             = 23, // You may only call down a stampede on Harold Lane.
        ERROR_REQUIRES_GAMMOTH_MAGNATAUR       = 24, // You may only use the Pouch of Crushed Bloodspore on Gammothra or other magnataur in the Bloodspore Plains and Gammoth.
        ERROR_MUST_BE_IN_RESURRECTION_CHAMBER  = 25, // Requires the magmawyrm resurrection chamber in the back of the Maw of Neltharion.
        ERROR_CANT_CALL_WINTERGARDE_HERE       = 26, // You may only call down a Wintergarde Gryphon in Wintergarde Keep or the Carrion Fields.
        ERROR_MUST_TARGET_WILHELM              = 27, // What are you doing? Only aim that thing at Wilhelm!
        ERROR_NOT_ENOUGH_HEALTH                = 28, // Not enough health!
        ERROR_NO_NEARBY_CORPSES                = 29, // There are no nearby corpses to use
        ERROR_TOO_MANY_GHOULS                  = 30, // You've created enough ghouls. Return to Gothik the Harvester at Death's Breach.
        ERROR_GO_FURTHER_FROM_SUNDERED_SHARD   = 31, // Your companion does not want to come here.  Go further from the Sundered Shard.
        ERROR_MUST_BE_IN_CAT_FORM              = 32, // Must be in Cat Form
        ERROR_MUST_BE_DEATH_KNIGHT             = 33, // Only Death Knights may enter Ebon Hold.
        ERROR_MUST_BE_IN_FERAL_FORM            = 34, // Must be in Cat Form, Bear Form, or Dire Bear Form
        ERROR_MUST_BE_NEAR_HELPLESS_VILLAGER   = 35, // You must be within range of a Helpless Wintergarde Villager.
        ERROR_CANT_TARGET_ELEMENTAL_MECHANICAL = 36, // You cannot target an elemental or mechanical corpse.
        ERROR_MUST_HAVE_USED_DALARAN_CRYSTAL   = 37, // This teleport crystal cannot be used until the teleport crystal in Dalaran has been used at least once.
        ERROR_YOU_ALREADY_HOLD_SOMETHING       = 38, // You are already holding something in your hand. You must throw the creature in your hand before picking up another.
        ERROR_YOU_DONT_HOLD_ANYTHING           = 39, // You don't have anything to throw! Find a Vargul and use Gymer Grab to pick one up!
        ERROR_MUST_BE_CLOSE_TO_VALDURAN        = 40, // Bouldercrag's War Horn can only be used within 10 yards of Valduran the Stormborn.
        ERROR_NO_PASSENGER                     = 41, // You are not carrying a passenger. There is nobody to drop off.
        ERROR_CANT_BUILD_MORE_VEHICLES         = 42, // You cannot build any more siege vehicles.
        ERROR_ALREADY_CARRYING_CRUSADER        = 43, // You are already carrying a captured Argent Crusader. You must return to the Argent Vanguard infirmary and drop off your passenger before you may pick up another.
        ERROR_CANT_DO_WHILE_ROOTED             = 44, // You can't do that while rooted.
        ERROR_REQUIRES_NEARBY_TARGET           = 45, // Requires a nearby target.
        ERROR_NOTHING_TO_DISCOVER              = 46, // Nothing left to discover.
        ERROR_NOT_ENOUGH_TARGETS               = 47, // No targets close enough to bluff.
        ERROR_CONSTRUCT_TOO_FAR                = 48, // Your Iron Rune Construct is out of range.
        ERROR_REQUIRES_GRAND_MASTER_ENGINEER   = 49, // Requires Grand Master Engineer
        ERROR_CANT_USE_THAT_MOUNT              = 50, // You can't use that mount.
        ERROR_NOONE_TO_EJECT                   = 51, // There is nobody to eject!
        ERROR_TARGET_MUST_BE_BOUND             = 52, // The target must be bound to you.
        ERROR_TARGET_MUST_BE_UNDEAD            = 53, // Target must be undead.
        ERROR_TARGET_TOO_FAR                   = 54, // You have no target or your target is too far away.
        ERROR_MISSING_DARK_MATTER              = 55, // Missing Reagents: Dark Matter
        ERROR_CANT_USE_THAT_ITEM               = 56, // You can't use that item
        ERROR_CANT_DO_WHILE_CYCYLONED          = 57, // You can't do that while Cycloned
        ERROR_TARGET_HAS_SCROLL                = 58, // Target is already affected by a scroll
        ERROR_POISON_TOO_STRONG                = 59, // That anti-venom is not strong enough to dispel that poison
        ERROR_MUST_HAVE_LANCE_EQUIPPED         = 60, // You must have a lance equipped.
        ERROR_MUST_BE_CLOSE_TO_MAIDEN          = 61, // You must be near the Maiden of Winter's Breath Lake.
        ERROR_LEARNED_EVERYTHING               = 62, // You have learned everything from that book
        ERROR_PET_IS_DEAD                      = 63, // Your pet is dead
        ERROR_NO_VALID_TARGETS                 = 64, // There are no valid targets within range.
        ERROR_GM_ONLY                          = 65, // Only GMs may use that. Your account has been reported for investigation.
        ERROR_REQUIRES_LEVEL_58                = 66, // You must reach level 58 to use this portal.
        ERROR_AT_HONOR_CAP                     = 67, // You already have the maximum amount of honor.
        ERROR_68                               = 68, // ""
        ERROR_69                               = 69, // ""
        ERROR_70                               = 70, // ""
        ERROR_71                               = 71, // ""
        ERROR_72                               = 72, // ""
        ERROR_73                               = 73, // ""
        ERROR_74                               = 74, // ""
        ERROR_MUST_HAVE_DEMONIC_CIRCLE         = 75, // You must have a demonic circle active.
        ERROR_AT_MAX_RAGE                      = 76, // You already have maximum rage
        ERROR_REQUIRES_350_ENGINEERING         = 77, // Requires Engineering (350)
        ERROR_SOUL_BELONGS_TO_LICH_KING        = 78, // Your soul belongs to the Lich King
        ERROR_ATTENDANT_HAS_PONY               = 79, // Your attendant already has an Argent Pony
        ERROR_80                               = 80, // ""
        ERROR_81                               = 81, // ""
        ERROR_82                               = 82, // ""
        ERROR_MUST_HAVE_FIRE_TOTEM             = 83, // You must have a Fire Totem active.
        ERROR_CANT_TARGET_VAMPIRES             = 84, // You may not bite other vampires.
        ERROR_PET_ALREADY_AT_YOUR_LEVEL        = 85, // Your pet is already at your level.
        ERROR_MISSING_ITEM_REQUIREMENS         = 86, // You do not meet the level requirements for this item.
        ERROR_TOO_MANY_ABOMINATIONS            = 87, // There are too many Mutated Abominations.
        ERROR_ALL_POTIONS_USED                 = 88, // The potions have all been depleted by Professor Putricide.
        ERROR_89                               = 89, // ""
        ERROR_REQUIRES_LEVEL_65                = 90, // Requires level 65
        ERROR_91                               = 91, // ""
        ERROR_92                               = 92, // ""
        ERROR_93                               = 93, // ""
        ERROR_94                               = 94, // ""
        ERROR_95                               = 95, // ""
        ERROR_MAX_NUMBER_OF_RECRUITS           = 96, // You already have the max number of recruits.
        ERROR_MAX_NUMBER_OF_VOLUNTEERS         = 97, // You already have the max number of volunteers.
        ERROR_FROSTMOURNE_RENDERED_RESSURECT   = 98, // Frostmourne has rendered you unable to ressurect.
        ERROR_CANT_MOUNT_WITH_SHAPESHIFT       = 99  // You can't mount while affected by that shapeshift.
    }

    enum ReputationRank
    {
        REP_HATED       = 0,
        REP_HOSTILE     = 1,
        REP_UNFRIENDLY  = 2,
        REP_NEUTRAL     = 3,
        REP_FRIENDLY    = 4,
        REP_HONORED     = 5,
        REP_REVERED     = 6,
        REP_EXALTED     = 7
    }

    enum DrunkenState
    {
        DRUNKEN_STATE_SOBER,
        DRUNKEN_STATE_TIPSY,
        DRUNKEN_STATE_DRUNK,
        DRUNKEN_STATE_SMASHED,
    }

    enum InstanceInfo
    {
        INSTANCE_INFO_DATA,
        INSTANCE_INFO_DATA64,
        INSTANCE_INFO_BOSS_STATE
    }

    enum PlayerClasses
    {
        CLASS_NONE          = 0,
        CLASS_WARRIOR       = 1,
        CLASS_PALADIN       = 2,
        CLASS_HUNTER        = 3,
        CLASS_ROGUE         = 4,
        CLASS_PRIEST        = 5,
        CLASS_DEATH_KNIGHT  = 6,
        CLASS_SHAMAN        = 7,
        CLASS_MAGE          = 8,
        CLASS_WARLOCK       = 9,
        //CLASS_UNK           = 10,
        CLASS_DRUID         = 11
    }

    enum Races
    {
        RACE_NONE               = 0,
        RACE_HUMAN              = 1,
        RACE_ORC                = 2,
        RACE_DWARF              = 3,
        RACE_NIGHTELF           = 4,
        RACE_UNDEAD_PLAYER      = 5,
        RACE_TAUREN             = 6,
        RACE_GNOME              = 7,
        RACE_TROLL              = 8,
        //RACE_GOBLIN             = 9,
        RACE_BLOODELF           = 10,
        RACE_DRAENEI            = 11
        //RACE_FEL_ORC            = 12,
        //RACE_NAGA               = 13,
        //RACE_BROKEN             = 14,
        //RACE_SKELETON           = 15,
        //RACE_VRYKUL             = 16,
        //RACE_TUSKARR            = 17,
        //RACE_FOREST_TROLL       = 18,
        //RACE_TAUNKA             = 19,
        //RACE_NORTHREND_SKELETON = 20,
        //RACE_ICE_TROLL          = 21
    }

    enum Difficulty
    {
        REGULAR_DIFFICULTY = 0,

        DUNGEON_DIFFICULTY_NORMAL = 0,
        DUNGEON_DIFFICULTY_HEROIC = 1,
        DUNGEON_DIFFICULTY_EPIC = 2,

        RAID_DIFFICULTY_10MAN_NORMAL = 0,
        RAID_DIFFICULTY_25MAN_NORMAL = 1,
        RAID_DIFFICULTY_10MAN_HEROIC = 2,
        RAID_DIFFICULTY_25MAN_HEROIC = 3
    }

    [Flags]
    enum SpawnMask
    {
        SPAWNMASK_CONTINENT = (1 << Difficulty.REGULAR_DIFFICULTY), // any maps without spawn modes

        SPAWNMASK_DUNGEON_NORMAL = (1 << Difficulty.DUNGEON_DIFFICULTY_NORMAL),
        SPAWNMASK_DUNGEON_HEROIC = (1 << Difficulty.DUNGEON_DIFFICULTY_HEROIC),
        SPAWNMASK_DUNGEON_ALL = (SPAWNMASK_DUNGEON_NORMAL | SPAWNMASK_DUNGEON_HEROIC),

        SPAWNMASK_RAID_10MAN_NORMAL = (1 << Difficulty.RAID_DIFFICULTY_10MAN_NORMAL),
        SPAWNMASK_RAID_25MAN_NORMAL = (1 << Difficulty.RAID_DIFFICULTY_25MAN_NORMAL),
        SPAWNMASK_RAID_NORMAL_ALL = (SPAWNMASK_RAID_10MAN_NORMAL | SPAWNMASK_RAID_25MAN_NORMAL),

        SPAWNMASK_RAID_10MAN_HEROIC = (1 << Difficulty.RAID_DIFFICULTY_10MAN_HEROIC),
        SPAWNMASK_RAID_25MAN_HEROIC = (1 << Difficulty.RAID_DIFFICULTY_25MAN_HEROIC),
        SPAWNMASK_RAID_HEROIC_ALL = (SPAWNMASK_RAID_10MAN_HEROIC | SPAWNMASK_RAID_25MAN_HEROIC),

        SPAWNMASK_RAID_ALL = (SPAWNMASK_RAID_NORMAL_ALL | SPAWNMASK_RAID_HEROIC_ALL)
    }
}
