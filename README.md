# PlayListGenerator

The main goal of this tool is to generate playlist file (m3u or xspf format).
These playlists are usable by winamp or vlc for example.
It can be any music or video files.

# Examples

For all the following example, imagine that you have these 4 mp3 files on your disk :

- c:\songs\albumA\songA1.mp3
- c:\songs\albumA\songA2.mp3
- c:\songs\albumB\songB1.mp3
- c:\songs\albumB\songB1.mp3

### Example 1 (basic usage - absolute path)
```
C:\>PlayListGenerator.exe "c:\songs\albumA\*.mp3" "playlist.m3u"
```

Will generate the following "c:\songs\albumA\playlist.m3u" file :

- c:\songs\albumA\songA1.mp3
- c:\songs\albumA\songA2.mp3

### Example 2 (basic usage - relative path)
```
C:\>PlayListGenerator.exe "c:\songs\albumA\*.mp3" "playlist.m3u" -R
```

Will generate the following "c:\songs\albumA\playlist.m3u" file :

- songA1.mp3
- songA2.mp3

### Example 3 (recursive usage - relative path)
```
C:\>PlayListGenerator.exe "c:\songs\*.mp3" "playlist.m3u" -S -R
```

Will generate the following "c:\songs\playlist.m3u" file :

- albumA\songA1.mp3
- albumA\songA2.mp3
- albumB\songB1.mp3
- albumB\songB1.mp3

### Example 4 (OnePlaylistByFolder usage - relative path)
```
C:\>PlayListGenerator.exe "c:\songs\*.mp3" "playlist.m3u" -R --OnePlaylistByFolder
```

Will generate the following files : 

1st file : "c:\songs\albumA\playlist.m3u" 

- songA1.mp3
- songA2.mp3

2nd file : "c:\songs\albumB\playlist.m3u" file :

- songB1.mp3
- songB1.mp3

### Example 5 (xspf file format)
```
C:\>PlayListGenerator.exe "c:\songs\albumA\*.mp3" "playlist.m3u" -F xspf
```
