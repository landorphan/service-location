@Check-In
Feature: WindowsPaths
	In order to reliably interact with the file systems of multiple platforms
	As a developer
	I want to be able to parse paths on multiple platforms correctly

Scenario Outline: Drive Rooted Paths
	Given I have the following path: <Path>
	 When I parse the path as a Windows Path
     And I evaluate the original form
	 Then I should receive a path object
	  And segment '0' should be: <Root>
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
#
# Path Segment Type Shorthand:
# {N} = NullSegment, {E} = EmptySegment, {R} = RootSegment, {D} = DeviceSegment, {/} = VolumelessRootSegment
# {V} = VolumeRelativeSegment, {U} = UncSegment, {G} = Segment, {.} = SelfSegmentk, {..} = ParentSegment
Examples: 
| Name               | Path                              | Length | Root        | Segment 1             | Segment 2    | Segment 3    | Anchor   | Status      |
# a null string can be parsed but will produce a null path (which is an illegal path)
| Null               | (null)                            | 1      | {N} (null)  | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Illegal     |
# an empty string can be parsed but will produce an empty path (which is an illegal path)
| Empty              | (empty)                           | 1      | {E} (empty) | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Illegal     |
| Volume Absolute    | C:`                               | 2      | {R} C:      | {E} (empty)           | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Volume Relative    | C:.`file.txt                      | 3      | {V} C:      | {.} .                 | {G} file.txt | {N} (null)   | Relative | Legal       |
| UNC                | ``server`share`dir`file.txt       | 4      | {U} server  | {G} share             | {G} dir      | {G} file.txt | Absolute | Legal       |
| Long Volume Abs    | ``?`C:`dir`file.txt               | 3      | {R} C:      | {G} dir               | {G} file.txt | {N} (null)   | Absolute | Legal       |
| Long UNC           | ``?`UNC`server`share`dir`file.txt | 4      | {U} server  | {G} share             | {G} dir      | {G} file.txt | Absolute | Legal       |
| Self Relative      | .`dir`file.txt                    | 3      | {.} .       | {G} dir               | {G} file.txt | {N} (null)   | Relative | Legal       |
| Parent Relative    | ..`dir`file.txt                   | 3      | {..} ..     | {G} dir               | {G} file.txt | {N} (null)   | Relative | Legal       |
# Empty segments will show up in the non-normalized for but will not be present in the normalized form
| Empty Abs Segment  | C:`dir``file.txt                  | 4      | {R} C:      | {G} dir               | {E} (empty)  | {G} file.txt | Absolute | Legal       |
| Empty Rel Segment  | .`dir``file.txt                   | 4      | {.} .       | {segment} dir         | {E} (empty)  | {G} file.txt | Relative | Legal       |
| Relative           | dir`file.txt                      | 2      | {G} dir     | {G} file.txt          | {N} (null)   | {N} (null)   | Relative | Legal       |
# The following represent reserved device paths and are legal (they open a stream to the device in question if it is present)
| CON                | CON                               | 1      | {D} CON     | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| PRN                | PRN                               | 1      | {D} PRN     | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| AUX                | AUX                               | 1      | {D} AUX     | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| NUL                | NUL                               | 1      | {D} NUL     | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| COM1               | COM1                              | 1      | {D} COM1    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| COM2               | COM2                              | 1      | {D} COM2    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| COM3               | COM3                              | 1      | {D} COM3    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| COM4               | COM4                              | 1      | {D} COM4    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| COM5               | COM5                              | 1      | {D} COM5    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| COM6               | COM6                              | 1      | {D} COM6    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| COM7               | COM7                              | 1      | {D} COM7    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| COM8               | COM8                              | 1      | {D} COM8    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| COM9               | COM9                              | 1      | {D} COM9    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| LPT1               | LPT1                              | 1      | {D} LPT1    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| LPT2               | LPT2                              | 1      | {D} LPT2    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| LPT3               | LPT3                              | 1      | {D} LPT3    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| LPT4               | LPT4                              | 1      | {D} LPT4    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| LPT5               | LPT5                              | 1      | {D} LPT5    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| LPT6               | LPT6                              | 1      | {D} LPT6    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| LPT7               | LPT7                              | 1      | {D} LPT7    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| LPT8               | LPT8                              | 1      | {D} LPT8    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| LPT9               | LPT9                              | 1      | {D} LPT9    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
# Long variants of the device paths are also allowed (with the same behavior as above)
| Long CON           | ``?`CON                           | 1      | {D} CON     | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long PRN           | ``?`PRN                           | 1      | {D} PRN     | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long AUX           | ``?`AUX                           | 1      | {D} AUX     | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long NUL           | ``?`NUL                           | 1      | {D} NUL     | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long COM1          | ``?`COM1                          | 1      | {D} COM1    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long COM2          | ``?`COM2                          | 1      | {D} COM2    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long COM3          | ``?`COM3                          | 1      | {D} COM3    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long COM4          | ``?`COM4                          | 1      | {D} COM4    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long COM5          | ``?`COM5                          | 1      | {D} COM5    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long COM6          | ``?`COM6                          | 1      | {D} COM6    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long COM7          | ``?`COM7                          | 1      | {D} COM7    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long COM8          | ``?`COM8                          | 1      | {D} COM8    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long COM9          | ``?`COM9                          | 1      | {D} COM9    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long LPT1          | ``?`LPT1                          | 1      | {D} LPT1    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long LPT2          | ``?`LPT2                          | 1      | {D} LPT2    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long LPT3          | ``?`LPT3                          | 1      | {D} LPT3    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long LPT4          | ``?`LPT4                          | 1      | {D} LPT4    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long LPT5          | ``?`LPT5                          | 1      | {D} LPT5    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long LPT6          | ``?`LPT6                          | 1      | {D} LPT6    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long LPT7          | ``?`LPT7                          | 1      | {D} LPT7    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long LPT8          | ``?`LPT8                          | 1      | {D} LPT8    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
| Long LPT9          | ``?`LPT9                          | 1      | {D} LPT9    | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
# Using a device path as a relitive path is actually legal (all device paths are absolute)
# NOTE: Unlike all other paths, Device Paths only ever have one segment regardless of what was provided
# Most other paths will keep unecissary components (example {E}) unless they are "explicitly" normalized to have those removed
| Rel CON            | ..`.`CON                          | 3      | {..} ..     | {.} .                 | {D} CON      | {N} (null)   | Absolute | Legal       |
# Using a device path as an absolute path is actually legal (all device paths are absolute)
| Abs Con            | C:`CON                            | 2      | {R} C:     | {D} CON            | {N} (null)   | {N} (null)   | Absolute | Legal       |
# Using a device path with a colon is in fact leagal
| Volume CON         | CON:                              | 1      | {D} CON     | {N} (null)            | {N} (null)   | {N} (null)   | Absolute | Legal       |
# Using a device path with an extention (as in a file name) is legal but highly discurouged (note this is a relative path because it is not a device path)
| Discuraged Rel NUL | .`NUL.txt                         | 2      | {.} .       | {G} NUL.txt           | {N} (null)   | {N} (null)   | Relative | Discouraged |
# Using an illegal character in a path is illegal
| Illegal Rel Astr   | .`foo*bar.txt                     | 2      | {.} .       | {G} foo*bar.txt       | {N} (null)   | {N} (null)   | Relative | Illegal     |
# After the long sentax, a question mark is illegal
| Illegal Rel Ques   | .`foo?bar.txt                     | 2      | {.} .       | {G} foo?bar.txt       | {N} (null)   | {N} (null)   | Relative | Illegal     |
# After the volume sentax, a colon is illegal
| Illegal Rel Colon  | .`foo:bar.txt                     | 2      | {.} .       | {Segmant} foo:bar.txt | {N} (null)   | {N} (null)   | Relative | Illegal     |
# NOTE: There are no pipe test cases here (because Gherkin uses that for the table, we will have to test those in scenarios and not ountlines)

