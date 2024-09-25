using UnityEngine;
using System.Collections.Generic;

/*
 * This static class is developped to convert a wav file (from a byte array) into a
 * UnityEngine AudioClip to be played dynamically.
 * 
 * The other function allows your application to pick any AudioClip with valid data
 * and convert its content into a wav file (to a byte array).
 * 
 * The way you read or save the files is up to you (depending on the platform).
 * 
 * eToile recommends using FileManagement to read and save files (OWP is already integrated):
 * https://assetstore.unity.com/packages/slug/67183
 * 
 * V1.1:
 * New feature: Automatic detection of valid headers for non WAV files.
 * 
 * V1.2:
 * BugFix: Failing when reading empty or corrupted files.
 * 
 * v1.3:
 * BugFix: Workaround to avoid AudioClip generating incorrect length (fix for loopable clips).
 * 
 * v1.4:
 * NewFeature: 24bit WAV compatibility.
 * NewFeature: 32bit WAV compatibility.
 * NewFeature: Simple method to combine AudioClips.
 * 
 * v1.5:
 * BugFix: Multi-channel AudioClip incomplete when saving.
 * 
 * v1.6:
 * NewFeature: Stereo to mono and mono to stereo resampling.
 */

public static class OpenWavParser
{
    /// <summary>Available audio resolutions</summary>
    public enum Resolution
    {
        _16bit = 16,
        _24bit = 24,
        _32bit = 32
    }

    ///<summary>Check if the provided file is a valid PCM file</summary
    public static bool IsWAVFile(byte[] wavFile)
    {
        if (wavFile.Length > 12)
        {
            byte[] data = new byte[4];
            System.Buffer.BlockCopy(wavFile, 0, data, 0, data.Length);
            string _chunkID = ByteArrayToString(data);
            System.Buffer.BlockCopy(wavFile, 8, data, 0, data.Length);
            string _format = ByteArrayToString(data);
            return (_chunkID == "RIFF" && _format == "WAVE");
        }
        return false;
    }
    /// <summary>Load audio file into AudioClip</summary>
    public static AudioClip ByteArrayToAudioClip(byte[] wavFile, string name = "", bool stream = false)
    {
        /* WAV file format:
         * 
         * size - Name              - (index)   Description.
         * 
         * 4    - ChunkID           - (0)       "RIFF"
         * 4    - ChunkSize         - (4)       file size minus 8 (RIFF(4) + ChunkSize(4)).
         * 4    - Format            - (8)       "WAVE"
         * 
         * 4    - Subchunk1ID       - (12)      "fmt "
         * 4    - Subchunk1Size     - (16)      16 bytes for PCM (bytes 20 to 36)
         * 2    - AudioFormat       - (20)      1 for PCM (other values implies some compression).
         * 2    - NumChannels       - (22)      Mono = 1, Stereo = 2, etc.
         * 4    - SampleRate        - (24)      8000, 22050, 44100, etc.
         * 4    - ByteRate          - (28)      == SampleRate * NumChannels * (BitsPerSample/8)
         * 2    - BlockAlign        - (32)      == NumChannels * (BitsPerSample/8)
         * 2    - BitsPerSample     - (34)      8 bits = 8, 16 bits = 16, etc.
         * (Here goes the extra data pointed by Subchunk1Size > 16)
         * 
         * 4    - Subchunk2ID       - (36)      "data"
         * 4    - Subchunk2Size     - (40)
         * Subchunk2Size (Data)     - (44)
         */

        // Check if the provided file is a valid PCM file:
        if (IsWAVFile(wavFile))
        {
            //int _chunkSize = System.BitConverter.ToInt32(wavFile, 4);                         // Not used.
            int _subchunk1Size = System.BitConverter.ToInt32(wavFile, 16);
            int _audioFormat = System.BitConverter.ToInt16(wavFile, 20);
            int _numChannels = System.BitConverter.ToInt16(wavFile, 22);
            int _sampleRate = System.BitConverter.ToInt32(wavFile, 24);
            //int _byteRate = System.BitConverter.ToInt32(wavFile, 28);                         // Not used.
            //int _blockAlign = System.BitConverter.ToInt16(wavFile, 32);                       // Not used.
            Resolution _bitsPerSample = (Resolution)System.BitConverter.ToInt16(wavFile, 34);
            // PCM WAV method:
            if (_audioFormat == 1)
            {
                // Find where data starts:
                int _dataIndex = 20 + _subchunk1Size;
                for (int i = _dataIndex; i < wavFile.Length; i++)
                {
                    if (wavFile[i] == 'd' && wavFile[i + 1] == 'a' && wavFile[i + 2] == 't' && wavFile[i + 3] == 'a')
                    {
                        _dataIndex = i + 4;     // "data" string size = 4
                        break;
                    }
                }
                // Data parameters:
                int _subchunk2Size = System.BitConverter.ToInt32(wavFile, _dataIndex);          // Data size (Subchunk2Size).
                _dataIndex += 4;                                                                // Subchunk2Size = 4
                int _sampleSize = (int)_bitsPerSample / 8;                                      // Size of a sample.
                int _sampleCount = _subchunk2Size / _sampleSize;                                // How many samples into data.
                // Data conversion:
                float[] _audioBuffer = new float[_sampleCount];                                 // Size for all available channels.
                for (int i = 0; i < _sampleCount; i++)
                {
                    int sampleIndex = _dataIndex + i * _sampleSize;
                    int intSample = 0;
                    float sample = 0;
                    switch(_bitsPerSample)
                    {
                        case Resolution._16bit:
                            intSample = System.BitConverter.ToInt16(wavFile, sampleIndex);
                            sample = intSample / 32767f;
                            break;
                        case Resolution._24bit:
                            intSample = System.BitConverter.ToInt32(new byte[] { 0, wavFile[sampleIndex], wavFile[sampleIndex + 1], wavFile[sampleIndex + 2] }, 0) >> 8;
                            sample = intSample / 8388607f;
                            break;
                        case Resolution._32bit:
                            intSample = System.BitConverter.ToInt32(wavFile, sampleIndex);
                            sample = intSample / 2147483647f;
                            break;
                    }
                    _audioBuffer[i] = sample;
                }
                // Create the AudioClip:
                AudioClip audioClip = AudioClip.Create(name, _sampleCount / _numChannels, _numChannels, _sampleRate, stream);
                audioClip.SetData(_audioBuffer, 0);
                return audioClip;
            }
            else
            {
                Debug.LogError("[OpenWavParser.ByteArrayToAudioClip] Compressed wav format not supported.");
                return null;
            }
        }
        else
        {
            Debug.LogError("[OpenWavParser.ByteArrayToAudioClip] Format not supported.");
            return null;
        }
    }
    ///<summary>Returns a wav file from any AudioClip</summary
    public static byte[] AudioClipToByteArray(AudioClip clip, Resolution res = Resolution._16bit)
    {
        // Clip content:
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);                                                               // The audio data in samples.
        // Write all data to byte array:
        List<byte> wavFile = new List<byte>();
        // RIFF header:
        int size = (int)res / 8;
        wavFile.AddRange(new byte[] { (byte)'R', (byte)'I', (byte)'F', (byte)'F' });            // "RIFF"
        wavFile.AddRange(System.BitConverter.GetBytes(samples.Length * size + 44 - 8));         // ChunkSize
        wavFile.AddRange(new byte[] { (byte)'W', (byte)'A', (byte)'V', (byte)'E' });            // "WAVE"
        wavFile.AddRange(new byte[] { (byte)'f', (byte)'m', (byte)'t', (byte)' ' });            // "fmt "
        wavFile.AddRange(System.BitConverter.GetBytes(16));                                     // Subchunk1Size (the next 16 bytes)
        wavFile.AddRange(System.BitConverter.GetBytes((ushort)1));                              // AudioFormat (1 for PCM)
        wavFile.AddRange(System.BitConverter.GetBytes((ushort)clip.channels));                  // NumChannels
        wavFile.AddRange(System.BitConverter.GetBytes(clip.frequency));                         // SampleRate
        wavFile.AddRange(System.BitConverter.GetBytes(clip.frequency * clip.channels * size));  // ByteRate
        wavFile.AddRange(System.BitConverter.GetBytes((ushort)(clip.channels * size)));         // BlockAlign
        wavFile.AddRange(System.BitConverter.GetBytes((ushort)res));                            // BitsPerSample
        wavFile.AddRange(new byte[] { (byte)'d', (byte)'a', (byte)'t', (byte)'a' });            // "data"
        wavFile.AddRange(System.BitConverter.GetBytes(samples.Length * size));                  // Subchunk2Size
        // Add the audio data in bytes:
        for (int i = 0; i < samples.Length; i++)
        {
            switch(res)
            {
                case Resolution._16bit:
                    short sample16 = (short)(samples[i] * 32767f);
                    wavFile.AddRange(System.BitConverter.GetBytes(sample16));
                    break;
                case Resolution._24bit:
                    int sample24 = Mathf.FloorToInt(samples[i] * 8388607);
                    byte[] data = System.BitConverter.GetBytes(sample24);
                    wavFile.AddRange(new byte[] { data[0], data[1], data[2] });
                    break;
                case Resolution._32bit:
                    int sample32 = (int)(samples[i] * 2147483647f);
                    wavFile.AddRange(System.BitConverter.GetBytes(sample32));
                    break;
            }
        }
        // Return the byte array to be saved:
        return wavFile.ToArray();
    }
    
    ///<summary>Converts to 8 bit characters only</summary>
    static string ByteArrayToString(byte[] content)
    {
        char[] chars = new char[content.Length];
        content.CopyTo(chars, 0);
        return new string(chars);
    }

    ///<summary>Combines all AudioClip from the array</summary>
    public static AudioClip Combine(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        int length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] != null)
                length += clips[i].samples * clips[i].channels;
        }

        float[] data = new float[length];
        length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] != null)
            {
                float[] buffer = new float[clips[i].samples * clips[i].channels];
                clips[i].GetData(buffer, 0);
                buffer.CopyTo(data, length);
                length += buffer.Length;
            }
        }

        AudioClip audioClip = null;
        if (length > 0)
        {
            audioClip = AudioClip.Create("AudioClip", length / 2, 2, 44100, false);
            audioClip.SetData(data, 0);
        }
        return audioClip;
    }
    ///<summary>Resamples a stereo clip to mono (Mixes both channels)</summary>
    public static AudioClip StereoToMono(AudioClip stereoClip, bool stream = false)
    {
        // Get stereo data:
        float[] _audioStereo = new float[stereoClip.samples * stereoClip.channels];
        stereoClip.GetData(_audioStereo, 0);
        // Mxx to mono data:
        float[] _audioBuffer = new float[stereoClip.samples];
        for(int s = 0; s < _audioBuffer.Length; s++)
        {
            _audioBuffer[s] = (float)((double)_audioStereo[s * 2] + _audioStereo[s * 2 + 1]) / 2f;
        }
        AudioClip audioClip = AudioClip.Create(stereoClip.name, _audioBuffer.Length, 1, stereoClip.frequency, stream);
        audioClip.SetData(_audioBuffer, 0);
        return audioClip;
    }
    ///<summary>Resamples a mono clip to stereo (Copies the mono channel)</summary>
    public static AudioClip MonoToStereo(AudioClip monoClip, bool stream = false)
    {
        // Get mono data:
        float[] _audioMono = new float[monoClip.samples];
        monoClip.GetData(_audioMono, 0);
        // Convert to stereo data:
        float[] _audioBuffer = new float[monoClip.samples * 2];
        for (int s = 0; s < _audioBuffer.Length; s += 2)
        {
            _audioBuffer[s] = _audioMono[s / 2];
            _audioBuffer[s + 1] = _audioMono[s / 2];
        }
        AudioClip audioClip = AudioClip.Create(monoClip.name, monoClip.samples, 2, monoClip.frequency, stream);
        audioClip.SetData(_audioBuffer, 0);
        return audioClip;
    }
}