using System.Collections.Generic;
using System.Linq;

namespace EliteJoystick.Common.Logic
{
    public static class KeyMap
    {
        public static List<KeyCode> ModifierKeys = new List<KeyCode>
        {
            new KeyCode("KEY_MOD_LCTRL", 0x01, "", "", "Left Control"),
            new KeyCode("KEY_MOD_LSHIFT", 0x02, "", "", "Left Shift"),
            new KeyCode("KEY_MOD_LALT", 0x04, "", "", "Left Alt"),
            new KeyCode("KEY_MOD_LMETA", 0x08, "", "", "Left Meta"),
            new KeyCode("KEY_MOD_RCTRL", 0x10, "", "", "Right Control"),
            new KeyCode("KEY_MOD_RSHIFT", 0x20, "", "", "Right Shift"),
            new KeyCode("KEY_MOD_RALT", 0x40, "", "", "Right Alt"),
            new KeyCode("KEY_MOD_RMETA", 0x80, "", "", "Right Meta"),
        };

        /// <summary>
        /// Scan codes - last N slots in the HID report(usually 6).
        /// 0x00 if no key pressed.
        ///
        /// If more than N keys are pressed, the HID reports
        /// KEY_ERR_OVF in all slots to indicate this condition.
        /// </summary>
        public static List<KeyCode> Keys = new List<KeyCode>
        {
            new KeyCode("KEY_NONE", 0x00, "", "", "No key pressed"),
            new KeyCode("KEY_ERR_OVF", 0x01, "", "", "Keyboard Error Roll Over - used for all slots if too many keys are pressed (\"Phantom key\")"),
            new KeyCode("KEY_POST_FAIL", 0x02, "", "", "Keyboard POST Fail"),
            new KeyCode("KEY_UNDEFINED", 0x03, "", "", "Keyboard Error Undefined"),
            new KeyCode("KEY_A", 0x04, "a", "A", "Keyboard a and A"),
            new KeyCode("KEY_B", 0x05, "b", "B", "Keyboard b and B"),
            new KeyCode("KEY_C", 0x06, "c", "C", "Keyboard c and C"),
            new KeyCode("KEY_D", 0x07, "d", "D", "Keyboard d and D"),
            new KeyCode("KEY_E", 0x08, "e", "E", "Keyboard e and E"),
            new KeyCode("KEY_F", 0x09, "f", "F", "Keyboard f and F"),
            new KeyCode("KEY_G", 0x0a, "g", "G", "Keyboard g and G"),
            new KeyCode("KEY_H", 0x0b, "h", "H", "Keyboard h and H"),
            new KeyCode("KEY_I", 0x0c, "i", "I", "Keyboard i and I"),
            new KeyCode("KEY_J", 0x0d, "j", "J", "Keyboard j and J"),
            new KeyCode("KEY_K", 0x0e, "k", "K", "Keyboard k and K"),
            new KeyCode("KEY_L", 0x0f, "l", "L", "Keyboard l and L"),
            new KeyCode("KEY_M", 0x10, "m", "M", "Keyboard m and M"),
            new KeyCode("KEY_N", 0x11, "n", "N", "Keyboard n and N"),
            new KeyCode("KEY_O", 0x12, "o", "O", "Keyboard o and O"),
            new KeyCode("KEY_P", 0x13, "p", "P", "Keyboard p and P"),
            new KeyCode("KEY_Q", 0x14, "q", "Q", "Keyboard q and Q"),
            new KeyCode("KEY_R", 0x15, "r", "R", "Keyboard r and R"),
            new KeyCode("KEY_S", 0x16, "s", "S", "Keyboard s and S"),
            new KeyCode("KEY_T", 0x17, "t", "T", "Keyboard t and T"),
            new KeyCode("KEY_U", 0x18, "u", "U", "Keyboard u and U"),
            new KeyCode("KEY_V", 0x19, "v", "V", "Keyboard v and V"),
            new KeyCode("KEY_W", 0x1a, "w", "W", "Keyboard w and W"),
            new KeyCode("KEY_X", 0x1b, "x", "X", "Keyboard x and X"),
            new KeyCode("KEY_Y", 0x1c, "y", "Y", "Keyboard y and Y"),
            new KeyCode("KEY_Z", 0x1d, "z", "Z", "Keyboard z and Z"),

            new KeyCode("KEY_1", 0x1e, "1", "!", "Keyboard 1 and !"),
            new KeyCode("KEY_2", 0x1f, "2", "@", "Keyboard 2 and @"),
            new KeyCode("KEY_3", 0x20, "3", "#", "Keyboard 3 and #"),
            new KeyCode("KEY_4", 0x21, "4", "$", "Keyboard 4 and $"),
            new KeyCode("KEY_5", 0x22, "5", "%", "Keyboard 5 and %"),
            new KeyCode("KEY_6", 0x23, "6", "^", "Keyboard 6 and ^"),
            new KeyCode("KEY_7", 0x24, "7", "&", "Keyboard 7 and &"),
            new KeyCode("KEY_8", 0x25, "8", "*", "Keyboard 8 and *"),
            new KeyCode("KEY_9", 0x26, "9", "(", "Keyboard 9 and ("),
            new KeyCode("KEY_0", 0x27, "0", ")", "Keyboard 0 and )"),

            new KeyCode("KEY_ENTER", 0x28, "", "", "Keyboard Return (ENTER)"),
            new KeyCode("KEY_ESC", 0x29, "", "", "Keyboard ESCAPE"),
            new KeyCode("KEY_BACKSPACE", 0x2a, "", "", "Keyboard DELETE (Backspace)"),
            new KeyCode("KEY_TAB", 0x2b, "", "", "Keyboard Tab"),
            new KeyCode("KEY_SPACE", 0x2c, " ", "", "Keyboard Spacebar"),
            new KeyCode("KEY_MINUS", 0x2d, "-", "_", "Keyboard - and _"),
            new KeyCode("KEY_EQUAL", 0x2e, "=", "+", "Keyboard = and +"),
            new KeyCode("KEY_LEFTBRACE", 0x2f, "[", "{", "Keyboard [ and {"),
            new KeyCode("KEY_RIGHTBRACE", 0x30, "]", "}", "Keyboard ] and }"),
            new KeyCode("KEY_BACKSLASH", 0x31, "\\", "|", "Keyboard \\ and |"),
            new KeyCode("KEY_HASHTILDE", 0x32, "", "", "Keyboard Non-US # and ~"),
            new KeyCode("KEY_SEMICOLON", 0x33, ";", ":", "Keyboard ; and :"),
            new KeyCode("KEY_APOSTROPHE", 0x34, "'", "\"", "Keyboard ' and \""),
            new KeyCode("KEY_GRAVE", 0x35, "`", "~", "Keyboard ` and ~"),
            new KeyCode("KEY_COMMA", 0x36, ",", "<", "Keyboard , and <"),
            new KeyCode("KEY_DOT", 0x37, ".", ">", "Keyboard . and >"),
            new KeyCode("KEY_SLASH", 0x38, "/", "?", "Keyboard / and ?"),
            new KeyCode("KEY_CAPSLOCK", 0x39, "", "", "Keyboard Caps Lock"),

            new KeyCode("KEY_F1", 0x3a, "", "", "Keyboard F1"),
            new KeyCode("KEY_F2", 0x3b, "", "", "Keyboard F2"),
            new KeyCode("KEY_F3", 0x3c, "", "", "Keyboard F3"),
            new KeyCode("KEY_F4", 0x3d, "", "", "Keyboard F4"),
            new KeyCode("KEY_F5", 0x3e, "", "", "Keyboard F5"),
            new KeyCode("KEY_F6", 0x3f, "", "", "Keyboard F6"),
            new KeyCode("KEY_F7", 0x40, "", "", "Keyboard F7"),
            new KeyCode("KEY_F8", 0x41, "", "", "Keyboard F8"),
            new KeyCode("KEY_F9", 0x42, "", "", "Keyboard F9"),
            new KeyCode("KEY_F10", 0x43, "", "", "Keyboard F10"),
            new KeyCode("KEY_F11", 0x44, "", "", "Keyboard F11"),
            new KeyCode("KEY_F12", 0x45, "", "", "Keyboard F12"),

            new KeyCode("KEY_SYSRQ", 0x46, "", "", "Keyboard Print Screen"),
            new KeyCode("KEY_SCROLLLOCK", 0x47, "", "", "Keyboard Scroll Lock"),
            new KeyCode("KEY_PAUSE", 0x48, "", "", "Keyboard Pause"),
            new KeyCode("KEY_INSERT", 0x49, "", "", "Keyboard Insert"),
            new KeyCode("KEY_HOME", 0x4a, "", "", "Keyboard Home"),
            new KeyCode("KEY_PAGEUP", 0x4b, "", "", "Keyboard Page Up"),
            new KeyCode("KEY_DELETE", 0x4c, "", "", "Keyboard Delete Forward"),
            new KeyCode("KEY_END", 0x4d, "", "", "Keyboard End"),
            new KeyCode("KEY_PAGEDOWN", 0x4e, "", "", "Keyboard Page Down"),
            new KeyCode("KEY_RIGHT", 0x4f, "", "", "Keyboard Right Arrow"),
            new KeyCode("KEY_LEFT", 0x50, "", "", "Keyboard Left Arrow"),
            new KeyCode("KEY_DOWN", 0x51, "", "", "Keyboard Down Arrow"),
            new KeyCode("KEY_UP", 0x52, "", "", "Keyboard Up Arrow"),

            new KeyCode("KEY_NUMLOCK", 0x53, "", "", "Keyboard Num Lock and Clear"),
            new KeyCode("KEY_KPSLASH", 0x54, "", "", "Keypad /"),
            new KeyCode("KEY_KPASTERISK", 0x55, "", "", "Keypad *"),
            new KeyCode("KEY_KPMINUS", 0x56, "", "", "Keypad -"),
            new KeyCode("KEY_KPPLUS", 0x57, "", "", "Keypad +"),
            new KeyCode("KEY_KPENTER", 0x58, "", "", "Keypad ENTER"),
            new KeyCode("KEY_KP1", 0x59, "", "", "Keypad 1 and End"),
            new KeyCode("KEY_KP2", 0x5a, "", "", "Keypad 2 and Down Arrow"),
            new KeyCode("KEY_KP3", 0x5b, "", "", "Keypad 3 and PageDn"),
            new KeyCode("KEY_KP4", 0x5c, "", "", "Keypad 4 and Left Arrow"),
            new KeyCode("KEY_KP5", 0x5d, "", "", "Keypad 5"),
            new KeyCode("KEY_KP6", 0x5e, "", "", "Keypad 6 and Right Arrow"),
            new KeyCode("KEY_KP7", 0x5f, "", "", "Keypad 7 and Home"),
            new KeyCode("KEY_KP8", 0x60, "", "", "Keypad 8 and Up Arrow"),
            new KeyCode("KEY_KP9", 0x61, "", "", "Keypad 9 and Page Up"),
            new KeyCode("KEY_KP0", 0x62, "", "", "Keypad 0 and Insert"),
            new KeyCode("KEY_KPDOT", 0x63, "", "", "Keypad . and Delete"),

            new KeyCode("KEY_102ND", 0x64, "", "", "Keyboard Non-US \\ and |"),
            new KeyCode("KEY_COMPOSE", 0x65, "", "", "Keyboard Application"),
            new KeyCode("KEY_POWER", 0x66, "", "", "Keyboard Power"),
            new KeyCode("KEY_KPEQUAL", 0x67, "", "", "Keypad ="),

            new KeyCode("KEY_F13", 0x68, "", "", "Keyboard F13"),
            new KeyCode("KEY_F14", 0x69, "", "", "Keyboard F14"),
            new KeyCode("KEY_F15", 0x6a, "", "", "Keyboard F15"),
            new KeyCode("KEY_F16", 0x6b, "", "", "Keyboard F16"),
            new KeyCode("KEY_F17", 0x6c, "", "", "Keyboard F17"),
            new KeyCode("KEY_F18", 0x6d, "", "", "Keyboard F18"),
            new KeyCode("KEY_F19", 0x6e, "", "", "Keyboard F19"),
            new KeyCode("KEY_F20", 0x6f, "", "", "Keyboard F20"),
            new KeyCode("KEY_F21", 0x70, "", "", "Keyboard F21"),
            new KeyCode("KEY_F22", 0x71, "", "", "Keyboard F22"),
            new KeyCode("KEY_F23", 0x72, "", "", "Keyboard F23"),
            new KeyCode("KEY_F24", 0x73, "", "", "Keyboard F24"),

            new KeyCode("KEY_OPEN", 0x74, "", "", "Keyboard Execute"),
            new KeyCode("KEY_HELP", 0x75, "", "", "Keyboard Help"),
            new KeyCode("KEY_PROPS", 0x76, "", "", "Keyboard Menu"),
            new KeyCode("KEY_FRONT", 0x77, "", "", "Keyboard Select"),
            new KeyCode("KEY_STOP", 0x78, "", "", "Keyboard Stop"),
            new KeyCode("KEY_AGAIN", 0x79, "", "", "Keyboard Again"),
            new KeyCode("KEY_UNDO", 0x7a, "", "", "Keyboard Undo"),
            new KeyCode("KEY_CUT", 0x7b, "", "", "Keyboard Cut"),
            new KeyCode("KEY_COPY", 0x7c, "", "", "Keyboard Copy"),
            new KeyCode("KEY_PASTE", 0x7d, "", "", "Keyboard Paste"),
            new KeyCode("KEY_FIND", 0x7e, "", "", "Keyboard Find"),
            new KeyCode("KEY_MUTE", 0x7f, "", "", "Keyboard Mute"),
            new KeyCode("KEY_VOLUMEUP", 0x80, "", "", "Keyboard Volume Up"),
            new KeyCode("KEY_VOLUMEDOWN", 0x81, "", "", "Keyboard Volume Down"),
            //", 0x82  Keyboard Locking Caps Lock
            //", 0x83  Keyboard Locking Num Lock
            //", 0x84  Keyboard Locking Scroll Lock
            new KeyCode("KEY_KPCOMMA", 0x85, "", "", "Keypad Comma"),
            new KeyCode("KEY_KPEQUAL_SIGN",  0x86, "", "", "Keypad Equal Sign"),
            new KeyCode("KEY_RO", 0x87, "", "", "Keyboard International1"),
            new KeyCode("KEY_KATAKANAHIRAGANA", 0x88, "", "", "Keyboard International2"),
            new KeyCode("KEY_YEN", 0x89, "", "", "Keyboard International3"),
            new KeyCode("KEY_HENKAN", 0x8a, "", "", "Keyboard International4"),
            new KeyCode("KEY_MUHENKAN", 0x8b, "", "", "Keyboard International5"),
            new KeyCode("KEY_KPJPCOMMA", 0x8c, "", "", "Keyboard International6"),
            //", 0x8d  Keyboard International7
            //", 0x8e  Keyboard International8
            //", 0x8f  Keyboard International9
            new KeyCode("KEY_HANGEUL", 0x90, "", "", "Keyboard LANG1"),
            new KeyCode("KEY_HANJA", 0x91, "", "", "Keyboard LANG2"),
            new KeyCode("KEY_KATAKANA", 0x92, "", "", "Keyboard LANG3"),
            new KeyCode("KEY_HIRAGANA", 0x93, "", "", "Keyboard LANG4"),
            new KeyCode("KEY_ZENKAKUHANKAKU", 0x94, "", "", "Keyboard LANG5"),
            //", 0x95  Keyboard LANG6
            //", 0x96  Keyboard LANG7
            //", 0x97  Keyboard LANG8
            //", 0x98  Keyboard LANG9
            //", 0x99  Keyboard Alternate Erase
            //", 0x9a  Keyboard SysReq/Attention
            //", 0x9b  Keyboard Cancel
            //", 0x9c  Keyboard Clear
            //", 0x9d  Keyboard Prior
            //", 0x9e  Keyboard Return
            //", 0x9f  Keyboard Separator
            //", 0xa0  Keyboard Out
            //", 0xa1  Keyboard Oper
            //", 0xa2  Keyboard Clear/Again
            //", 0xa3  Keyboard CrSel/Props
            //", 0xa4  Keyboard ExSel

            //", 0xb0  Keypad 00
            //", 0xb1  Keypad 000
            //", 0xb2  Thousands Separator
            //", 0xb3  Decimal Separator
            //", 0xb4  Currency Unit
            //", 0xb5  Currency Sub-unit
            new KeyCode("KEY_KPLEFTPAREN", 0xb6, "", "", "Keypad ("),
            new KeyCode("KEY_KPRIGHTPAREN", 0xb7, "", "", "Keypad )"),
            //", 0xb8  Keypad {
            //", 0xb9  Keypad }
            //", 0xba  Keypad Tab
            //", 0xbb  Keypad Backspace
            //", 0xbc  Keypad A
            //", 0xbd  Keypad B
            //", 0xbe  Keypad C
            //", 0xbf  Keypad D
            //", 0xc0  Keypad E
            //", 0xc1  Keypad F
            //", 0xc2  Keypad XOR
            //", 0xc3  Keypad ^
            //", 0xc4  Keypad %
            //", 0xc5  Keypad <
            //", 0xc6  Keypad >
            //", 0xc7  Keypad &
            //", 0xc8  Keypad &&
            //", 0xc9  Keypad |
            //", 0xca  Keypad ||
            //", 0xcb  Keypad :
            //", 0xcc  Keypad #
            //", 0xcd  Keypad Space
            //", 0xce  Keypad @
            //", 0xcf  Keypad !
            //", 0xd0  Keypad Memory Store
            //", 0xd1  Keypad Memory Recall
            //", 0xd2  Keypad Memory Clear
            //", 0xd3  Keypad Memory Add
            //", 0xd4  Keypad Memory Subtract
            //", 0xd5  Keypad Memory Multiply
            //", 0xd6  Keypad Memory Divide
            //", 0xd7  Keypad +/-
            //", 0xd8  Keypad Clear
            //", 0xd9  Keypad Clear Entry
            //", 0xda  Keypad Binary
            //", 0xdb  Keypad Octal
            //", 0xdc  Keypad Decimal
            //", 0xdd  Keypad Hexadecimal

            new KeyCode("KEY_LEFTCTRL", 0xe0, "", "", "Keyboard Left Control"),
            new KeyCode("KEY_LEFTSHIFT", 0xe1, "", "", "Keyboard Left Shift"),
            new KeyCode("KEY_LEFTALT", 0xe2, "", "", "Keyboard Left Alt"),
            new KeyCode("KEY_LEFTMETA", 0xe3, "", "", "Keyboard Left GUI"),
            new KeyCode("KEY_RIGHTCTRL", 0xe4, "", "", "Keyboard Right Control"),
            new KeyCode("KEY_RIGHTSHIFT", 0xe5, "", "", "Keyboard Right Shift"),
            new KeyCode("KEY_RIGHTALT", 0xe6, "", "", "Keyboard Right Alt"),
            new KeyCode("KEY_RIGHTMETA", 0xe7, "", "", "Keyboard Right GUI"),

            new KeyCode("KEY_MEDIA_PLAYPAUSE", 0xe8, "", "", "Media Play/Pause"),
            new KeyCode("KEY_MEDIA_STOPCD", 0xe9, "", "", "Media Stop CD"),
            new KeyCode("KEY_MEDIA_PREVIOUSSONG", 0xea, "", "", "Media Previous"),
            new KeyCode("KEY_MEDIA_NEXTSONG", 0xeb, "", "", "Media Next"),
            new KeyCode("KEY_MEDIA_EJECTCD", 0xec, "", "", "Media Eject"),
            new KeyCode("KEY_MEDIA_VOLUMEUP", 0xed, "", "", "Media Volume +"),
            new KeyCode("KEY_MEDIA_VOLUMEDOWN", 0xee, "", "", "Media Volume -"),
            new KeyCode("KEY_MEDIA_MUTE", 0xef, "", "", "Media Mute"),
            new KeyCode("KEY_MEDIA_WWW", 0xf0, "", "", "Media Browser"),
            new KeyCode("KEY_MEDIA_BACK", 0xf1, "", "", "Media Back"),
            new KeyCode("KEY_MEDIA_FORWARD", 0xf2, "", "", "Media Forward"),
            new KeyCode("KEY_MEDIA_STOP", 0xf3, "", "", "Media Stop"),
            new KeyCode("KEY_MEDIA_FIND", 0xf4, "", "", "Media Search"),
            new KeyCode("KEY_MEDIA_SCROLLUP", 0xf5, "", "", "Media Scroll Up"),
            new KeyCode("KEY_MEDIA_SCROLLDOWN", 0xf6, "", "", "Media Scroll Down"),
            new KeyCode("KEY_MEDIA_EDIT", 0xf7, "", "", "Media Edit"),
            new KeyCode("KEY_MEDIA_SLEEP", 0xf8, "", "", "Media Sleep"),
            new KeyCode("KEY_MEDIA_COFFEE", 0xf9, "", "", "Media Coffee"),
            new KeyCode("KEY_MEDIA_REFRESH", 0xfa, "", "", "Media Refresh"),
            new KeyCode("KEY_MEDIA_CALC", 0xfb, "", "", "Media Calculator")
        };

        public static IDictionary<string, KeyCode> LowerCaseMap =
            Keys.Where(x => x.Value.Length > 0).ToDictionary(k => k.Value, v => v);

        public static IDictionary<string, KeyCode> UpperCaseMap =
            Keys.Where(x => x.ShiftedValue.Length > 0).ToDictionary(k => k.ShiftedValue, v => v);

        public static IDictionary<string, KeyCode> KeyNameMap =
            Keys.Where(x => x.Name.Length > 0).ToDictionary(k => k.Name, v => v);

        public static IDictionary<string, KeyCode> ModifierKeyNameMap =
            ModifierKeys.Where(x => x.Name.Length > 0).ToDictionary(k => k.Name, v => v);
    }
}
