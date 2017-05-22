using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Specifies the possible light modes of the Strategic Commander.
    /// </summary>
    [Serializable]
    public enum SwscLight
    {
        /// <summary>
        /// Active Light on button 1.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button1Flash"/>.
        /// </remarks>
        Button1 = 0x0001,

        /// <summary>
        /// Flashing Light on button 1.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button1"/>.
        /// </remarks>
        Button1Flash = 0x0002,

        /// <summary>
        /// Active Light on button 2.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button2Flash"/>.
        /// </remarks>
        Button2 = 0x0004,

        /// <summary>
        /// Flashing Light on button 2.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button2"/>.
        /// </remarks>
        Button2Flash = 0x0008,

        /// <summary>
        /// Active Light on button 3.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button3Flash"/>.
        /// </remarks>
        Button3 = 0x0010,

        /// <summary>
        /// Flashing Light on button 3.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button3"/>.
        /// </remarks>
        Button3Flash = 0x0020,

        /// <summary>
        /// Active Light on button 4.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button4Flash"/>.
        /// </remarks>
        Button4 = 0x0040,

        /// <summary>
        /// Flashing Light on button 4.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button4"/>.
        /// </remarks>
        Button4Flash = 0x0080,

        /// <summary>
        /// Active Light on button 5.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button5Flash"/>.
        /// </remarks>
        Button5 = 0x0100,

        /// <summary>
        /// Flashing Light on button 5.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button5"/>.
        /// </remarks>
        Button5Flash = 0x0200,

        /// <summary>
        /// Active Light on button 6.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button6Flash"/>.
        /// </remarks>
        Button6 = 0x0400,

        /// <summary>
        /// Flashing Light on button 6.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Button6"/>.
        /// </remarks>
        Button6Flash = 0x0800,

        /// <summary>
        /// Active Light on Record button.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.RecordFlash"/>.
        /// </remarks>
        Record = 0x1000,

        /// <summary>
        /// Flashing Light on Record button.
        /// </summary>
        /// <remarks>
        /// This value is incompatible with <see cref="SwscLight.Record"/>.
        /// </remarks>
        RecordFlash = 0x2000,
    }
}
