using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class WavUtility
{
    public static byte[] FromAudioClip(AudioClip clip, out string filepath, bool saveToFile = true)
    {
        string filename = $"voice_{DateTime.Now:yyyyMMdd_HHmmss}.wav";
        filepath = Path.Combine(Application.persistentDataPath, filename);

        MemoryStream stream = new MemoryStream();
        WriteWav(clip, stream);
        byte[] bytes = stream.ToArray();

        if (saveToFile)
        {
            File.WriteAllBytes(filepath, bytes);
        }

        return bytes;
    }

    private static void WriteWav(AudioClip clip, Stream stream)
    {
        int sampleCount = clip.samples * clip.channels;
        float[] samples = new float[sampleCount];
        clip.GetData(samples, 0);

        byte[] wavData = ConvertTo16Bit(samples);
        BinaryWriter writer = new BinaryWriter(stream);

        int byteRate = clip.frequency * clip.channels * 2;

        // 헤더 작성
        writer.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));
        writer.Write(36 + wavData.Length);
        writer.Write(System.Text.Encoding.UTF8.GetBytes("WAVEfmt "));
        writer.Write(16);
        writer.Write((short)1); // PCM
        writer.Write((short)clip.channels);
        writer.Write(clip.frequency);
        writer.Write(byteRate);
        writer.Write((short)(clip.channels * 2));
        writer.Write((short)16); // 16비트
        writer.Write(System.Text.Encoding.UTF8.GetBytes("data"));
        writer.Write(wavData.Length);
        writer.Write(wavData);
    }

    private static byte[] ConvertTo16Bit(float[] samples)
    {
        byte[] bytes = new byte[samples.Length * 2];
        int rescale = 32767;

        for (int i = 0; i < samples.Length; i++)
        {
            short s = (short)(samples[i] * rescale);
            byte[] byteArr = BitConverter.GetBytes(s);
            bytes[i * 2] = byteArr[0];
            bytes[i * 2 + 1] = byteArr[1];
        }

        return bytes;
    }
}