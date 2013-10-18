using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAI_Editor.Classes
{
    [Flags]
    public enum NpcFlags1
    {
        UNIT_NPC_FLAG_GOSSIP = 0x00000001,       // 100%
        UNIT_NPC_FLAG_QUESTGIVER = 0x00000002,       // guessed, probably ok
        UNIT_NPC_FLAG_UNK1 = 0x00000004,
        UNIT_NPC_FLAG_UNK2 = 0x00000008,
        UNIT_NPC_FLAG_TRAINER = 0x00000010,       // 100%
        UNIT_NPC_FLAG_TRAINER_CLASS = 0x00000020,       // 100%
        UNIT_NPC_FLAG_TRAINER_PROFESSION = 0x00000040,       // 100%
        UNIT_NPC_FLAG_VENDOR = 0x00000080,       // 100%
        UNIT_NPC_FLAG_VENDOR_AMMO = 0x00000100,       // 100%, general goods vendor
        UNIT_NPC_FLAG_VENDOR_FOOD = 0x00000200,       // 100%
        UNIT_NPC_FLAG_VENDOR_POISON = 0x00000400,       // guessed
        UNIT_NPC_FLAG_VENDOR_REAGENT = 0x00000800,       // 100%
        UNIT_NPC_FLAG_REPAIR = 0x00001000,       // 100%
        UNIT_NPC_FLAG_FLIGHTMASTER = 0x00002000,       // 100%
        UNIT_NPC_FLAG_SPIRITHEALER = 0x00004000,       // guessed
        UNIT_NPC_FLAG_SPIRITGUIDE = 0x00008000,       // guessed
        UNIT_NPC_FLAG_INNKEEPER = 0x00010000,       // 100%
        UNIT_NPC_FLAG_BANKER = 0x00020000,       // 100%
        UNIT_NPC_FLAG_PETITIONER = 0x00040000,       // 100% 0xC0000 = guild petitions, 0x40000 = arena team petitions
        UNIT_NPC_FLAG_TABARDDESIGNER = 0x00080000,       // 100%
        UNIT_NPC_FLAG_BATTLEMASTER = 0x00100000,       // 100%
        UNIT_NPC_FLAG_AUCTIONEER = 0x00200000,       // 100%
        UNIT_NPC_FLAG_STABLEMASTER = 0x00400000,       // 100%
        UNIT_NPC_FLAG_GUILD_BANKER = 0x00800000,       // cause client to send 997 opcode
        UNIT_NPC_FLAG_SPELLCLICK = 0x01000000,       // cause client to send 1015 opcode (spell click)
        UNIT_NPC_FLAG_PLAYER_VEHICLE = 0x02000000        // players with mounts that have vehicle data should have it set
    }
}
