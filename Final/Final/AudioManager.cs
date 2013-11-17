using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Final
{
    public class AudioManager
    {
        GameScreen activeScreen;
        ScreenManager screenManager;
        List<String> songs;
        SoundEffect spooky;
        SoundEffect currentSound;
        bool playing = false;

        public AudioManager(GameScreen activeScreen)
        {
            this.activeScreen = activeScreen;
        }

        public AudioManager(ScreenManager sm) {
            screenManager = sm;
            songs = new List<string>(new String[] {"OST","first"});
            currentSound = sm.Game.Content.Load<SoundEffect>("sound/theme");
            spooky = sm.Game.Content.Load<SoundEffect>("sound/spooky");
        }

        public void update(int x, int y)
        {
            if (x == 0 && y == 0 && !playing)
            {
                currentSound.Play();
                playing = true;
            }
            else if (x == 6 && y == 1)
            {
                currentSound = screenManager.Game.Content.Load<SoundEffect>("sound/menutheme");
            }
        }

        public void playSound(SoundEffect sound)
        {
            sound.Play();
        }

        public void playSpooky()
        {
            spooky.Play();
        }

        public void setCurrentSong(SoundEffect song)
        {
            currentSound = song;
            currentSound.Play();
            playing = true;
        }

        public void loadContent()
        {

        }

    }
}
