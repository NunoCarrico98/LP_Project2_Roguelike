﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Roguelike
{
    /// <summary>
    /// Class that manages High Scores.
    /// </summary>
    [Serializable()]
    public class HighScoreManager
    {

        /// <summary>
        /// Create a List to hold highscores.
        /// </summary>
        public List<Tuple<string, float>> Highscores { get; private set; }

        /// <summary>
        /// Create variable to hold the filename.
        /// </summary>
        private readonly string filename = "";

        /// <summary>
        /// Constructor to initialise List.
        /// </summary>
        /// <param name="filename">Filename given by user.</param>
        public HighScoreManager(string filename = "HighScores.txt")
        {
            // Initialise filename with filename given
            this.filename = filename;

            // If File does not exist, initialise empty list
            if (!File.Exists(filename))
            {
                Highscores = new List<Tuple<string, float>>(10);
                AddScore("Default", 0);
            }
            // If file exists
            else
            {
                // Initialise List
                Highscores = new List<Tuple<string, float>>(10);
                // Read All Lines from file
                string[] text = File.ReadAllLines(filename);

                // Cycle through all the lines
                for (int i = 1; i < text.Length; i++)
                {
                    // Separate lines according to accepted format
                    string[] subStrings = text[i].Split(':');

                    // If format is incorrect or the second subString cannot be
                    // converted to float
                    if (subStrings.Length != 2 ||
                        !Single.TryParse(subStrings[1], out float score))
                    {
                        // Send Error message
                        throw new InvalidOperationException($"The format of " +
                            $"the file '{filename}' is not correct.");
                    }

                    // Save name from the first subString
                    string name = subStrings[0];

                    // Add highscore to list
                    Highscores.Add(new Tuple<string, float>(name, score));
                }

                // Sort the elements of the list in descending order
                SortList();
            }
        }

        /// <summary>
        /// Method that Adds a new Highscore.
        /// </summary>
        /// <param name="name">Name of player.</param>
        /// <param name="score">Score of player.</param>
        public void AddScore(string name, float score)
        {
            // Create and instantiate a new object to hold the new highscore
            Tuple<string, float> newScore = new Tuple<string, float>(name, score);

            // Add highscore to list
            Highscores.Add(newScore);

            // Sort the elements of the list in descending order
            SortList();

            // If list has more than 10 elements
            if (Highscores.Count > 10)
            {
                // Remove last item on list
                Highscores.RemoveAt(Highscores.Count - 1);
            }
        }

        /// <summary>
        /// Method to save highscores to file.
        /// </summary>
        public void Save()
        {
            // Create variable to hold all text from list
            string text = "Name:Score\n";
            // For each element of the list
            foreach (Tuple<string, float> highscore in Highscores)
            {
                // Add text with Name and score
                text += $"{highscore.Item1}:{highscore.Item2}\n";
            }

            // Write all the Highscores on the specified file
            File.WriteAllText(filename, text);
        }

        /// <summary>
        /// Overriding method ToString().
        /// </summary>
        /// <returns>Returns a fully formmated table with all high scores.</returns>
        public override string ToString()
        {
            // Instantiate a new instance of StringBuilder
            // StringBuilder provides a more efficient of joining strings together
            StringBuilder sb = new StringBuilder("Name            |Score\n");
            sb.Append("-------------------------\n");

            // For each element of the list
            foreach (Tuple<string, float> highscore in Highscores)
            {
                // Add text with Name and score in table format
                sb.AppendFormat($"{highscore.Item1,-16}|{highscore.Item2,-16}\n");
            }

            // Return string with formated table
            return sb.ToString();
        }

        /// <summary>
        /// Method to sort the list.
        /// </summary>
        private void SortList()
        {
            // Sort the elements of the list in a descending order
            for (int i = 0; i <= Highscores.Count - 1; i++)
            {
                for (int j = 0; j < Highscores.Count - 1; j++)
                {
                    // Use Bubble Sort to sort the list
                    if (Highscores[j].Item2 < Highscores[i].Item2)
                    {
                        Tuple<string, float> temp = Highscores[i];
                        Highscores[i] = Highscores[j];
                        Highscores[j] = temp;
                    }
                }
            }
        }
    }
}