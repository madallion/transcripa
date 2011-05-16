/* 
 * transcripa
 * http://code.google.com/p/transcripa/
 * 
 * Copyright 2011, Bryan McKelvey
 * Licensed under the MIT license
 * http://www.opensource.org/licenses/mit-license.php
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace transcripa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Transcriber transcriber;
        public Accents accents;
        private const string windowTitle = "ipa";

        public MainWindow()
        {
            int comboBoxIndex = 0;
            int counter = 0;
            string currentLanguage = Properties.Settings.Default.CurrentLanguage;
            string currentInput = Properties.Settings.Default.CurrentInput;

            InitializeComponent();
            textBoxInput.Text = currentInput;
            textBoxInput.SelectionStart = currentInput.Length;

            transcriber = new Transcriber();
            accents = new Accents();

            // Write available languages to the combo box
            foreach (string language in transcriber.Languages)
            {
                comboBoxLanguage.Items.Add(language);
                if (language == currentLanguage)
                {
                    comboBoxIndex = counter;
                }
                counter++;
            }

            // Select the first item if it exists
            if (transcriber.Length != 0)
            {
                comboBoxLanguage.SelectedIndex = comboBoxIndex;
            }

            Transcribe();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.CurrentInput = textBoxInput.Text;
            Properties.Settings.Default.Save();
        }

        private void textBoxInput_KeyUp(object sender, KeyEventArgs e)
        {
            string text = textBoxInput.Text;
            int start = textBoxInput.SelectionStart;

            if (e.Key == Key.Escape)
            {
                Close();       
            }
            else if ((e.Key < Key.A || e.Key > Key.Z))
            {
                int textLength = text.Length;

                for (int i = accents.MaxLength; i > 1; i--)
                {
                    if (textLength >= i && start >= i)
                    {
                        string unparsed = text.Substring(start - i, i);
                        string parsed = accents.Apply(unparsed);
                        if (parsed != unparsed)
                        {
                            text = text.Remove(start - i, i).Insert(start - i, parsed);
                            textBoxInput.Text = text;
                            textBoxInput.SelectionStart = start - 1;
                            break;
                        }
                    }
                }
            }

            Transcribe();
        }

        private void Transcribe()
        {
            textBoxOutput.Text = transcriber.Transcribe(textBoxInput.Text);
        }

        private void comboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string currentLanguage = comboBoxLanguage.SelectedValue.ToString();
            transcriber.Load(currentLanguage);
            Title = string.Format("{0} - {1}", windowTitle, currentLanguage);
            Properties.Settings.Default.CurrentLanguage = currentLanguage;
            textBoxInput.Focus();
        }
    }
}
