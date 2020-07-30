# GenesisEdit

An all in one editor for the Sega Genesis

# Why not [SGDK](https://github.com/Stephane-D/SGDK)?

It's your choice if you would rather code in C or assembly!

Personally I think SGDK is an AMAZING project that you should try out!

# Features

## Macros

GenesisEdit has macros like %IF% and %SPRITE% to make coding way easier!

## Automatic Sprite/Background Compilation

Sprites and Backgrounds will automatically compile into the `CODE.S` file

### Complications while making image converter

**Backgrounds are converted to this format:**
|   |   |   |
|---|---|---|
| 1 | 2 | 3 |
| 4 | 5 | 6 |
| 7 | 8 | 9 |

**But sprites go like this:**
|   |   |   |
|---|---|---|
| 1 | 4 | 7 |
| 2 | 5 | 8 |
| 3 | 6 | 9 |

Seems simple, but GenesisEdit internally represents characters (8x8 groups of pixels) in the background format to make things simple.

#### Solution

Get an array of indexes into the character array that when the indexes are swapped with characters it converts it to sprite format.

`{ 1, 4, 7, 2, 5, 8, 3, 6, 9 }`

##### The Pattern

Given `w`, the width of the image in characters, and going from top down, each element is a multiple of `w` + some value that increases by one on each row.

This means we can create a list from 0 to the number of characters - 1.

Luckily, `Enumerable.Range(0, chars.Length)` gives us what we want.

Then we can sort it by its divisibility by `w`, so:

 - 0 % 3 = 0
 - 1 % 3 = 1
 - 2 % 3 = 2
 - 3 % 3 = 0
 - 4 % 3 = 1
 - 5 % 3 = 2
 - ...

Becomes:
 - 0 % 3 = 0
 - 3 % 3 = 0
 - 1 % 3 = 1
 - 4 % 3 = 1
 - 2 % 3 = 2
 - 5 % 3 = 2
 - ...

etc.

Puting this into a table:

|   |   |   |
|---|---|---|
| 0 | 3 | 6 |
| 1 | 4 | 7 |
| 2 | 5 | 8 |

#### Putting it all together (in C#)

```
int[] indexes = Enumerable.Range(0, chars.Count).OrderBy(i => i % (b.Width / 8)).ToArray();
Bitmap[] temp = new Bitmap[chars.Count];
for (int i = 0; i < chars.Count; i++)
{
	UpdateProgress($"Transforming {i + 1} -> {indexes[i] + 1}");
	temp[i] = chars[indexes[i]];
	PROGRESS++;
}
return temp.ToList();
```


## Variables

Variables can be created with 3 different types: Byte, Word, and Long, each having a different size (anything greater than one makes it an array)

They can only have a name that matches this regex: `[A-Za-z_][A-Za-z_0-9]*`

For those who don't understand regex, names must start with a letter or an underscore, then you can put letters, numbers, and underscores after the first character.

Variables are referenced by typing `%<NAME>%`

EX:
```
ADD.W   #42,D0
MOVE.W  D0,%MyVariable%
```

When compiled it becomes:
```
ADD.W   #42,D0
MOVE.W  D0,GE_USER_MyVariable
```
The definition at the top of `CODE.S`:
```
GE_USER_MyVariable    RC.W  1
```

# Credits

**HUGE** credit to a Youtuber named GameHut for inspiring this project.

The `SYSTEM.S` file was used from the tutorial, along with snippets from the main `GAMEHUT.S` file

**\> [Tutorial Playlist](https://www.youtube.com/watch?v=rnCPBcSRt7Y&list=PLi29TNPrdbwLmUjiVvLLrRky7cXrlSIYr) \<**
