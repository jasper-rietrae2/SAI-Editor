namespace SAI_Editor
{
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
    }

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
        SMART_EVENT_TARGET_CASTING = 13,
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

    public enum SmartCastFlags
    {
        SMARTCAST_INTERRUPT_PREVIOUS = 0x01,
        SMARTCAST_TRIGGERED = 0x02,
        SMARTCAST_AURA_NOT_PRESENT = 0x20,
    }

    public enum ReactStates
    {
        REACT_PASSIVE = 0,
        REACT_DEFENSIVE = 1,
        REACT_AGGRESSIVE = 2,
    }

    public enum SmartScriptRespawnCondition
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
        SMARTAI_TEMPLATE_BASIC,
        SMARTAI_TEMPLATE_CASTER,
        SMARTAI_TEMPLATE_TURRET,
        SMARTAI_TEMPLATE_PASSIVE,
        SMARTAI_TEMPLATE_CAGED_GO_PART,
        SMARTAI_TEMPLATE_CAGED_NPC_PART,
    }

    public enum GoFlags : uint
    {
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

    public enum UnitFieldBytes1Type
    {
        UnitStandStateType,
        UnitStandFlags,
        UnitBytes1_Flags,
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

    public enum UnitStandFlags : uint
    {
        UNIT_STAND_FLAGS_UNK1         = 0x01,
        UNIT_STAND_FLAGS_CREEP        = 0x02,
        UNIT_STAND_FLAGS_UNTRACKABLE  = 0x04,
        UNIT_STAND_FLAGS_UNK4         = 0x08,
        UNIT_STAND_FLAGS_UNK5         = 0x10,
        UNIT_STAND_FLAGS_ALL          = 0xFF
    }

    public enum UnitBytes1_Flags : uint
    {
        UNIT_BYTE1_FLAG_ALWAYS_STAND    = 0x01,
        UNIT_BYTE1_FLAG_HOVER           = 0x02,
        UNIT_BYTE1_FLAG_UNK_3           = 0x04,
        UNIT_BYTE1_FLAG_ALL             = 0xFF
    }

    public enum SmartEventFlags : uint
    {
        SMART_EVENT_FLAG_NOT_REPEATABLE     = 0x01,
        SMART_EVENT_FLAG_DIFFICULTY_0       = 0x02,
        SMART_EVENT_FLAG_DIFFICULTY_1       = 0x04,
        SMART_EVENT_FLAG_DIFFICULTY_2       = 0x08,
        SMART_EVENT_FLAG_DIFFICULTY_3       = 0x10,
        SMART_EVENT_FLAG_DEBUG_ONLY         = 0x80,
    }

    public enum UnitFlags : uint
    {
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

    public enum UnitFlags2 : uint
    {
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

    public enum SmartPhaseMasks
    {
        SMART_EVENT_PHASE_ALWAYS = 0,
        SMART_EVENT_PHASE_1 = 1,
        SMART_EVENT_PHASE_2 = 2,
        SMART_EVENT_PHASE_3 = 3,
        SMART_EVENT_PHASE_4 = 4,
        SMART_EVENT_PHASE_5 = 5,
        SMART_EVENT_PHASE_6 = 6,
    }
}
