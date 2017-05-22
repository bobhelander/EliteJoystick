﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Overrides the <see cref="Controller"/> class to provides specific events for
    /// the management of a 'Microsoft Sidewinder Game Voice'.
    /// </summary>
    public sealed class CHPedalsController : Controller
    {
        /// <summary>
        /// The vendor id for the Sidewinder Game Voice device.
        /// </summary>
        public const int VendorId = 0x068e;

        /// <summary>
        /// The product id for the Sidewinder Game Voice device.
        /// </summary>
        public const int ProductId = 0xc501;

        /// <summary>
        /// The stored state of the controller.
        /// </summary>
        private CHPedalsStatus previousRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwgvController"/> class.
        /// </summary>
        /// <param name="devicePath">
        /// The path of the device.
        /// </param>
        private CHPedalsController(string devicePath)
            : base(devicePath)
        {
            this.Read += this.OnRead;
        }

        /// <summary>
        /// Occurs when the profile changed.
        /// </summary>
        public event EventHandler<ProfileChangedEventArgs> ProfileChanged;

        /// <summary>
        /// Occurs when a button is pressed.
        /// </summary>
        public event EventHandler<ButtonEventArgs> ButtonDown;

        /// <summary>
        /// Occurs when a button is released.
        /// </summary>
        public event EventHandler<ButtonEventArgs> ButtonUp;

        /// <summary>
        /// Occurs when the controller is moved.
        /// </summary>
        public event EventHandler<MoveEventArgs> Move;

        /// <summary>
        /// Occurs when the controller is turned.
        /// </summary>
        public event EventHandler<RotateEventArgs> Rotate;

        /// <summary>
        /// Retrieves all the active Strategic Commander controllers.
        /// </summary>
        /// <returns>The active Strategic Commander controllers.</returns>
        public static ICollection<CHPedalsController> RetrieveAll()
        {
            ICollection<CHPedalsController> result = new LinkedList<CHPedalsController>();
            foreach (string devicePath in Controller.RetrieveAllDevicePath(VendorId, ProductId))
            {
                result.Add(new CHPedalsController(devicePath));
            }

            return result;
        }

        /// <summary>
        /// Initializes the controller for reads.
        /// </summary>
        public override void Initialize()
        {
            this.previousRead = CHPedalsStatus.Empty;
            base.Initialize();
        }

        /// <summary>
        /// Defines the enabled lights on the controller.
        /// </summary>
        /// <param name="lights">The enabled lights on the controller.</param>
        public void SetLights(params SwscLight[] lights)
        {
            short lightsValue = (short)lights.Sum(l => (int)l);
            byte[] values = new byte[this.FeatureLength];
            values[1] = (byte)(lightsValue & 0x00ff);
            values[2] = (byte)(lightsValue >> 8);
            this.Feature = values;
        }

        /// <summary>
        /// Compares the current read with the previous one to determine the updates.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The details of the event.</param>
        private void OnRead(object sender, ReadEventArgs e)
        {
            CHPedalsStatus previous = this.previousRead;
            CHPedalsStatus current = CHPedalsStatus.Create(e.Values);

            // Trace the received data in both binary and rich formats
            if (Logger.Default.IsVerbose())
            {
                Logger.Default.Verbose("{0:x8} {1}", e.Values.Reverse().Aggregate(0L, (l, b) => l << 8 | b), current);
            }

            // Check whether the profile change
            //if (previous.Profile != current.Profile)
            //{
            //    this.OnProfileChanged(current.Profile);
            //}

            // Check whether the position change
            if (previous.X != current.X || previous.Y != current.Y)
            {
                this.OnMove(current.X, current.Y);
            }

            // Check whether the rotation change
            if (previous.R != current.R)
            {
                this.OnRotate(current.R);
            }

            // Check whether the buttons change
            //var upButtons = previous.DownButtons.Except(current.DownButtons);
            //foreach (SwscButton button in upButtons)
            //{
            //    this.OnButtonUp(button);
            //}

            //var downButtons = current.DownButtons.Except(previous.DownButtons);
            //foreach (SwscButton button in downButtons)
            //{
            //    this.OnButtonDown(button);
            //}

            // Store the current status
            this.previousRead = current;
        }

        /// <summary>
        /// Raised the <see cref="SwgvController.ProfileChanged"/> event.
        /// </summary>
        /// <param name="profile">The new profile.</param>
        private void OnProfileChanged(int profile)
        {
            if (this.ProfileChanged != null)
            {
                this.ProfileChanged(this, new ProfileChangedEventArgs(profile));
            }
        }

        /// <summary>
        /// Raised the <see cref="SwgvController.ButtonUp"/> event.
        /// </summary>
        /// <param name="button">The released button.</param>
        private void OnButtonUp(SwscButton button)
        {
            if (this.ButtonUp != null)
            {
                this.ButtonUp(this, new ButtonEventArgs(button, false));
            }
        }

        /// <summary>
        /// Raised the <see cref="SwgvController.ButtonDown"/> event.
        /// </summary>
        /// <param name="button">The pressed button.</param>
        private void OnButtonDown(SwscButton button)
        {
            if (this.ButtonDown != null)
            {
                this.ButtonDown(this, new ButtonEventArgs(button, true));
            }
        }

        /// <summary>
        /// Raised the <see cref="SwgvController.Move"/> event.
        /// </summary>
        /// <param name="x">The x part of the position.</param>
        /// <param name="y">The y part of the position.</param>
        private void OnMove(int x, int y)
        {
            if (this.Move != null)
            {
                this.Move(this, new MoveEventArgs(x, y));
            }
        }

        /// <summary>
        /// Raised the <see cref="SwgvController.Rotate"/> event.
        /// </summary>
        /// <param name="r">The current rotation level.</param>
        private void OnRotate(int r)
        {
            if (this.Rotate != null)
            {
                this.Rotate(this, new RotateEventArgs(r));
            }
        }
    }
}
