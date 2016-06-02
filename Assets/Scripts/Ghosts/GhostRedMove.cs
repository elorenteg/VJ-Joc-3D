using UnityEngine;
using System.Collections;

public class GhostRedMove : GhostMove
{
    private static int MAX_PATHFINDERS_CHASING_PACMAN = 10;
    private int timeChasingPacman;
    private bool haveSeenPacman;
    private int newSection;
    private int[] sections = { LevelCreator.SECTION_TOP_LEFT, LevelCreator.SECTION_TOP_RIGHT,
        LevelCreator.SECTION_BOTTOM_RIGHT, LevelCreator.SECTION_BOTTOM_LEFT };
    private int currentSection;

    public GhostRedMove() { }

    protected override void initSpecificGhost()
    {
        haveSeenPacman = false;
        newSection = LevelCreator.SectionTile(tileX, tileZ);
    }

    // Random with but chasing Pacman if he sees him
    public override void chasingPacman(int[][] Map)
    {
        if (!haveSeenPacman && CanSeePacman(Map))
        {
            haveSeenPacman = true;
            timeChasingPacman = MAX_PATHFINDERS_CHASING_PACMAN;
        }

        if (haveSeenPacman && timeChasingPacman > 0) base.chasingPacman(Map);
        else
        {
            bool baseIsValid = false;
            int actualSection = LevelCreator.SectionTile(tileX, tileZ);
            if (LevelCreator.AreSameSection(actualSection, newSection)) UpdateNewSection();

            int tx, tz;
            do
            {
                LevelCreator.TileInSection(tileX, tileZ, newSection, out tx, out tz);
            } while (!isValid(Map, tx, tz, baseIsValid));

            currentPath = pathBFS(Map, tileX, tileZ, tx, tz, baseIsValid);
        }

        --timeChasingPacman;

        if (timeChasingPacman == 0) haveSeenPacman = false;
    }

    private void UpdateNewSection()
    {
        float rand = Random.value;
        if (rand <= 0.5) currentSection = (currentSection + 1) % sections.Length;
        else
        {
            rand = Random.value;
            int newcurrentSection = (int)(rand / 0.25);
            newcurrentSection = newcurrentSection % sections.Length;
            if (newcurrentSection == currentSection) currentSection = (currentSection + 1) % sections.Length;
        }
        newSection = sections[currentSection];
    }
}
