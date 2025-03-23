using System.IO;

public class MediaPlayer
{
    static System.Media.SoundPlayer soundPlayer;

    public MediaPlayer(byte[] buffer)
    {
        var memoryStream = new MemoryStream(buffer, true);
        soundPlayer = new System.Media.SoundPlayer(memoryStream);
    }

    public void Play()
    {
        soundPlayer.Play();
    }

    public static void Play(byte[] buffer)
    {
        var memoryStream = new MemoryStream(buffer, true);

        memoryStream.Position = 0;

        using (System.Media.SoundPlayer sound = new System.Media.SoundPlayer(memoryStream))
        {
            sound.Play();
        }

        return;
     
        soundPlayer = new System.Media.SoundPlayer(memoryStream);

        soundPlayer.Stream.Seek(0, SeekOrigin.Begin);
        soundPlayer.Stream.Write(buffer, 0, buffer.Length);
        soundPlayer.Play();
    }
}