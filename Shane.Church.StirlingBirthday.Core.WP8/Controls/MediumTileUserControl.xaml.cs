﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Shane.Church.Utility.Core.WP;
using System.Windows.Media;
using System.Threading.Tasks;

namespace Shane.Church.StirlingBirthday.Core.WP.Controls
{
    public partial class MediumTileUserControl : UserControl
    {
        public MediumTileUserControl()
        {
            InitializeComponent();
        }

        public async Task ToTileAsync(string Path)
        {
            // Need to call these, otherwise the contents aren't rendered correctly.
            this.Measure(new Size(336, 336));
            this.Arrange(new Rect(0, 0, 336, 336));

            WriteableBitmap bitmap = new WriteableBitmap(this, new TranslateTransform());

            await Imaging.SaveImageAsync(bitmap, Path, Imaging.ImageType.Png);
        }

    }
}
