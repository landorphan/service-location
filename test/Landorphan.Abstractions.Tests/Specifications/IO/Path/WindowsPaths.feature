@Check-In
Feature: WindowsPaths
	In order to reliably interact with the file systems of multiple platforms
	As a developer
	I want to be able to parse paths on multiple platforms correctly

Scenario Outline: Drive Rooted Paths
	Given I have the following path
	 When I parse the path as a Windows Path
	 Then I should receive a path object
	  And the segment "root" should be <Root>
	  And the segment "1" should be <Segment 1>
	  And the segment "2" should be <Segment 2>
	  And the segment "3" should be <Segment 3>
	  And the path should be anchored to <Anchor>
	  And the parse status should be <Status>
	  And the segment length should be <Length>
# NOTES:
# Per the spec, the following characters are illegal (anywere in the path)
# ILLEGAL CHARACTERS: Less Than (<), Greater Than (>), Double Quote ("), Pipe (|), Asterisk (*)
# 
# Per the spec, the following characters always represent a Path Separator regarless of location and can not be part of the path
# Foward Slash (/), Back Slash (\)
#
# Per the spec, the following characters are reserved and have special meaning.  They are only legal in or before the first segmant
# Colon (:) - Legal in first segment, Question Mark (?) - Legal only before the first segment (when using "long" sentax)
Examples: 
| Name               | Path                              | Length | Root            | Segment 1             | Segment 2          | Segment 3          | Anchor   | Status      |
# a null string can be parsed but will produce a null path (which is an illegal path)
| Null               | (null)                            | 1      | {Null} (null)   | (null)                | (null)             | (null)             | Absolute | Illegal     |
# an empty string can be parsed but will produce an empty path (which is an illegal path)
| Empty              | (empty)                           | 1      | {Empty} (empty) | (null)                | (null)             | (null)             | Absolute | Illegal     |
| Volume Absolute    | C:\                               | 1      | {Root} C        | (null)                | (null)             | (null)             | Absolute | Legal       |
| Volume Relative    | C:.\file.txt                      | 2      | {Volume} C      | {Self} .              | {Segment} file.txt | (null)             | Relative | Legal       |
| UNC                | \\server\share\dir\file.txt       | 4      | {Server} server | {Segment} share       | {Segment} dir      | {Segment} file.txt | Absolute | Legal       |
| Long Volume Abs    | \\?\C:\dir\file.txt               | 3      | {Root} C        | {Segment} dir         | {Segment} file.txt | (null)             | Absolute | Legal       |
| Long UNC           | \\?\UNC\server\share\dir\file.txt | 4      | {Server} server | {Segment} share       | {Segment} dir      | {Segment} file.txt | Absolute | Legal       |
| Self Relative      | .\dir\file.txt                    | 3      | {Self} .        | {Segment} dir         | {Segment} file.txt | (null)             | Relative | Legal       |
| Parent Relative    | ..\dir\file.txt                   | 3      | {Parent} ..     | {Segment} dir         | {Segment} file.txt | (null)             | Relative | Legal       |
# Empty segments will show up in the non-normalized for but will not be present in the normalized form
| Empty Abs Segment  | C:\dir\\file.txt                  | 4      | {Root} C        | {Segment} dir         | {Empty} (empty)    | {Segment} file.txt | Absolute | Legal       |
| Empty Rel Segment  | .\dir\\file.txt                   | 4      | {Self} .        | {segment} dir         | {Empty} (empty)    | {Segment} file.txt | Relative | Legal       |
| Relative           | dir\file.txt                      | 2      | {Segment} dir   | {Segment} file.txt    | (null)             | (null)             | Relative | Legal       |
# The following represent reserved device paths and are legal (they open a stream to the device in question if it is present)
| CON                | CON                               | 1      | {Device} CON    | (null)                | (null)             | (null)             | Absolute | Legal       |
| PRN                | PRN                               | 1      | {Device} PRN    | (null)                | (null)             | (null)             | Absolute | Legal       |
| AUX                | AUX                               | 1      | {Device} AUX    | (null)                | (null)             | (null)             | Absolute | Legal       |
| NUL                | NUL                               | 1      | {Device} NUL    | (null)                | (null)             | (null)             | Absolute | Legal       |
| COM1               | COM1                              | 1      | {Device} COM1   | (null)                | (null)             | (null)             | Absolute | Legal       |
| COM2               | COM2                              | 1      | {Device} COM2   | (null)                | (null)             | (null)             | Absolute | Legal       |
| COM3               | COM3                              | 1      | {Device} COM3   | (null)                | (null)             | (null)             | Absolute | Legal       |
| COM4               | COM4                              | 1      | {Device} COM4   | (null)                | (null)             | (null)             | Absolute | Legal       |
| COM5               | COM5                              | 1      | {Device} COM5   | (null)                | (null)             | (null)             | Absolute | Legal       |
| COM6               | COM6                              | 1      | {Device} COM6   | (null)                | (null)             | (null)             | Absolute | Legal       |
| COM7               | COM7                              | 1      | {Device} COM7   | (null)                | (null)             | (null)             | Absolute | Legal       |
| COM8               | COM8                              | 1      | {Device} COM8   | (null)                | (null)             | (null)             | Absolute | Legal       |
| COM9               | COM9                              | 1      | {Device} COM9   | (null)                | (null)             | (null)             | Absolute | Legal       |
| LPT1               | LPT1                              | 1      | {Device} LPT1   | (null)                | (null)             | (null)             | Absolute | Legal       |
| LPT2               | LPT2                              | 1      | {Device} LPT2   | (null)                | (null)             | (null)             | Absolute | Legal       |
| LPT3               | LPT3                              | 1      | {Device} LPT3   | (null)                | (null)             | (null)             | Absolute | Legal       |
| LPT4               | LPT4                              | 1      | {Device} LPT4   | (null)                | (null)             | (null)             | Absolute | Legal       |
| LPT5               | LPT5                              | 1      | {Device} LPT5   | (null)                | (null)             | (null)             | Absolute | Legal       |
| LPT6               | LPT6                              | 1      | {Device} LPT6   | (null)                | (null)             | (null)             | Absolute | Legal       |
| LPT7               | LPT7                              | 1      | {Device} LPT7   | (null)                | (null)             | (null)             | Absolute | Legal       |
| LPT8               | LPT8                              | 1      | {Device} LPT8   | (null)                | (null)             | (null)             | Absolute | Legal       |
| LPT9               | LPT9                              | 1      | {Device} LPT9   | (null)                | (null)             | (null)             | Absolute | Legal       |
# Long variants of the device paths are also allowed (with the same behavior as above)
| Long CON           | \\?\CON                           | 1      | {Device} CON    | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long PRN           | \\?\PRN                           | 1      | {Device} PRN    | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long AUX           | \\?\AUX                           | 1      | {Device} AUX    | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long NUL           | \\?\NUL                           | 1      | {Device} NUL    | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long COM1          | \\?\COM1                          | 1      | {Device} COM1   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long COM2          | \\?\COM2                          | 1      | {Device} COM2   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long COM3          | \\?\COM3                          | 1      | {Device} COM3   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long COM4          | \\?\COM4                          | 1      | {Device} COM4   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long COM5          | \\?\COM5                          | 1      | {Device} COM5   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long COM6          | \\?\COM6                          | 1      | {Device} COM6   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long COM7          | \\?\COM7                          | 1      | {Device} COM7   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long COM8          | \\?\COM8                          | 1      | {Device} COM8   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long COM9          | \\?\COM9                          | 1      | {Device} COM9   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long LPT1          | \\?\LPT1                          | 1      | {Device} LPT1   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long LPT2          | \\?\LPT2                          | 1      | {Device} LPT2   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long LPT3          | \\?\LPT3                          | 1      | {Device} LPT3   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long LPT4          | \\?\LPT4                          | 1      | {Device} LPT4   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long LPT5          | \\?\LPT5                          | 1      | {Device} LPT5   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long LPT6          | \\?\LPT6                          | 1      | {Device} LPT6   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long LPT7          | \\?\LPT7                          | 1      | {Device} LPT7   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long LPT8          | \\?\LPT8                          | 1      | {Device} LPT8   | (null)                | (null)             | (null)             | Absolute | Legal       |
| Long LPT9          | \\?\LPT9                          | 1      | {Device} LPT9   | (null)                | (null)             | (null)             | Absolute | Legal       |
# Using a device path as a relitive path is actually legal (all device paths are absolute)
# NOTE: Unlike all other paths, Device Paths only ever have one segment regardless of what was provided
# Most other paths will keep unecissary components (example {Empty}) unless they are "explicitly" normalized to have those removed
| Rel CON            | ..\.\CON                          | 1      | {Device} CON    | (null)                | (null)             | (null)             | Absolute | Legal       |
# Using a device path as an absolute path is actually legal (all device paths are absolute)
| Abs Con            | C:\CON                            | 1      | {Device} CON    | (null)                | (null)             | (null)             | Absolute | Legal       |
# Using a device path with a colon is in fact leagal
| Volume CON         | CON:                              | 1      | {Device} CON    | (null)                | (null)             | (null)             | Absolute | Legal       |
# Using a device path with an extention (as in a file name) is legal but highly discurouged (note this is a relative path because it is not a device path)
| Discuraged Rel NUL | .\NUL.txt                         | 2      | {Self} .        | {Segment} NUL.txt     | (null)             | (null)             | Relative | Discouraged |
# Using an illegal character in a path is illegal
| Illegal Rel Astr   | .\foo*bar.txt                     | 2      | {Self} .        | {Segment} foo*bar.txt | (null)             | (null)             | Relative | Illegal     |
# After the long sentax, a question mark is illegal
| Illegal Rel Ques   | .\foo?bar.txt                     | 2      | {Self} .        | {Segment} foo?bar.txt | (null)             | (null)             | Relative | Illegal     |
# After the volume sentax, a colon is illegal
| Illegal Rel Colon  | .\foo:bar.txt                     | 2      | {Self} .        | {Segmant} foo:bar.txt | (null)             | (null)             | Relative | Illegal     |
# NOTE: There are no pipe test cases here (because Gherkin uses that for the table, we will have to test those in scenarios and not ountlines)

