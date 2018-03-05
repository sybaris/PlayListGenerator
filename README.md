[![Build Status](https://travis-ci.org/sybaris/PlayListGenerator.svg?branch=master)](https://travis-ci.org/sybaris/PlayListGenerator)
# PlayListGenerator

The main goal of this tool is to generate playlist file (m3u or xspf format).
These playlists are usable by winamp or vlc for example.
It can be any music or video files.

# Downloads
<a href="https://github.com/sybaris/PlayListGenerator/releases/latest">[ Download ]</a>
<a href="https://github.com/sybaris/PlayListGenerator/releases/latest" rel="nofollow" style="vertical-align: -webkit-baseline-middle;"> <img src=https://img.shields.io/github/downloads/sybaris/PlayListGenerator/latest/total.svg?maxAge=86400 alt="Github Releases (latest)"></img></a>

# Examples

For all the following example, imagine that you have these 4 mp3 files on your disk :

- c:\songs\albumA\songA1.mp3
- c:\songs\albumA\songA2.mp3
- c:\songs\albumB\songB1.mp3
- c:\songs\albumB\songB1.mp3

![alt text](https://github.com/sybaris/PlayListGenerator/blob/master/docs/Example0.jpg)

### Example 1 (basic usage - absolute path)
```
C:\>PlayListGenerator.exe "c:\songs\albumA\*.mp3" "playlist.m3u"
```

Will generate the following "c:\songs\albumA\playlist.m3u" file :

- c:\songs\albumA\songA1.mp3
- c:\songs\albumA\songA2.mp3

![alt text](https://github.com/sybaris/PlayListGenerator/blob/master/docs/Example1.jpg)

### Example 2 (basic usage - relative path)
```
C:\>PlayListGenerator.exe "c:\songs\albumA\*.mp3" "playlist.m3u" -R
```

Will generate the following "c:\songs\albumA\playlist.m3u" file :

- songA1.mp3
- songA2.mp3

![alt text](https://github.com/sybaris/PlayListGenerator/blob/master/docs/Example2.jpg)

### Example 3 (recursive usage - relative path)
```
C:\>PlayListGenerator.exe "c:\songs\*.mp3" "playlist.m3u" -S -R
```

Will generate the following "c:\songs\playlist.m3u" file :

- albumA\songA1.mp3
- albumA\songA2.mp3
- albumB\songB1.mp3
- albumB\songB1.mp3

![alt text](https://github.com/sybaris/PlayListGenerator/blob/master/docs/Example3.jpg)

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

![alt text](https://github.com/sybaris/PlayListGenerator/blob/master/docs/Example4.jpg)

### Example 5 (xspf file format)
```
C:\>PlayListGenerator.exe "c:\songs\albumA\*.mp3" "playlist.xspf" -F xspf
```

![alt text](https://github.com/sybaris/PlayListGenerator/blob/master/docs/Example5.jpg)