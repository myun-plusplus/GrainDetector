﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrainDetector
{
    public partial class MainForm
    {
        private ImageForm imageForm;

        private ImageDisplay imageDisplay;
        private RangeSelect rangeSelect;

        private bool _isImageFormOpened;
        private bool isImageFormOpened
        {
            get
            {
                return _isImageFormOpened;
            }
            set
            {
                _isImageFormOpened = value;
                validateControls();
            }
        }

        private FormState.ActionMode _actionMode;
        private FormState.ActionMode actionMode
        {
            get
            {
                return _actionMode;
            }
            set
            {
                _actionMode = value;
                validateControls();
                if (isImageFormOpened)
                {
                    this.imageForm.ActionMode = value;
                }
            }
        }

        private Bitmap _targetImage;
        private Bitmap targetImage
        {
            get
            { 
                return _targetImage;
            }
            set
            {
                if (_targetImage != null && value != _targetImage)
                {
                    _targetImage.Dispose();
                }
                _targetImage = value;
            }
        }

        public MainForm()
        {
            imageDisplay = new ImageDisplay();
            rangeSelect = new RangeSelect(imageDisplay);

            InitializeComponent();
            this.rangeSelectBindingSource.DataSource = rangeSelect;

            isImageFormOpened = false;
            actionMode = FormState.ActionMode.None;
#if DEBUG
            this.filePathTextBox.Text = @"D:\Projects\GrainDetector\sample2.jpg";
#endif
        }

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (this.imageForm != null && !this.imageForm.IsDisposed)
            {
                this.imageForm.Dispose();
            }
            this.imageForm = null;

            imageDisplay.Image = null;
            targetImage = null;

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void openImageForm()
        {
            this.imageForm = new ImageForm(imageDisplay, rangeSelect);
            this.imageForm.Location = new Point(this.Location.X + 300, this.Location.Y);
            this.imageForm.ActionMode = FormState.ActionMode.None;
            this.imageForm.FormClosing += imageForm_FormClosing;

            this.imageForm.Show();
        }

        private void closeImageForm()
        {
            if (this.imageForm != null && !this.imageForm.IsDisposed)
            {
                this.imageForm.Close();
            }
            this.imageForm = null;
        }

        private void updateValidation()
        {
            this.lowerXNumericUpDown.Maximum = targetImage.Width - 1;
            this.lowerYNumericUpDown.Maximum = targetImage.Height - 1;
            this.upperXNumericUpDown.Maximum = targetImage.Width - 1;
            this.upperYNumericUpDown.Maximum = targetImage.Height - 1;
        }

        private void validateControls()
        {
            this.SuspendLayout();

            if (isImageFormOpened)
            {
                if (actionMode == FormState.ActionMode.None)
                {
                    this.rangeSelectPanel.Enabled = true;
                    this.shownImageSelectCLB.Enabled = true;
                    this.zoomInButton.Enabled = true;
                    this.zoomOutButton.Enabled = true;
                    this.imageSaveButton.Enabled = true;
                }
                else if (actionMode == FormState.ActionMode.ImageRangeSelect)
                {
                    this.rangeSelectPanel.Enabled = true;
                    this.shownImageSelectCLB.Enabled = false;
                    this.zoomInButton.Enabled = true;
                    this.zoomOutButton.Enabled = true;
                    this.imageSaveButton.Enabled = false;
                }
            }
            else
            {
                this.rangeSelectPanel.Enabled = false;
                this.shownImageSelectCLB.Enabled = false;
                this.zoomInButton.Enabled = false;
                this.zoomOutButton.Enabled = false;
                this.imageSaveButton.Enabled = false;
            }

            this.ResumeLayout(false);
        }
    }
}