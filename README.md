# Swipe Flash

Swipe Flash is a flashcard app whose goal is to transform raw lists of learning material into a satisfying and engaging learning experience. The application has a simple UI which creates a smooth user experience.

In order to learn a vocabulary list, the user simply has to swipe each card left or right depending on whether the card was guessed correctly. The application will use a Leitner system combined with a forgetting curve handling to make sure the flashcards are learned in the best way possible.

To improve the quality of the learning, a daily revision is recommended.

# Managing flashcards

Flashcards are divided into families, e.g. "English to Spanish" or "Capitals of the world". These families can be found by going to settings -> Manage Flashcards... where they can be edited, enabled/disabled or removed entirely.

Families all have two categories (one for each side of the cards), these categories (e.g. "Spanish" or "English") all have a logo which is displayed on the top right corner of the card's opposite side to indicate what category the expected answer falls into. This logo must be unicode (emoji) to be accepted. Microsoft's emojis are currently limited, therefore some logos might not be displayed correctly.

# Adding flashcards

The Add Flashcards... button allows the user to import flashcards from a file. Currently only .txt and .sff files are accepted.

SFF is a custom extension for Swipe Flash cards which contains both category and flashcards data for a given family. They can be exported by clicking the edit (eye) button on any flashcard family and clicking Export Flashcards...

The articles section of the categories is used exclusively to remove articles when searching for relevant illustrations, this improves the quality of the results. Images are currently searched using Unsplash, therefore appropriate illustrations may not be found for languages other than english.

# Create flashcards from TXT file

Flashcards can be created by importing a TXT file, given that the file follows certain specifications
1. The different cards each take a single line of the file
2. The formatting of the text is constant (meaning it must follow a certain logic)

Lines can be ignored by adding a pattern to the Ignore Lines with Pattern text box. Examples:
"#*" ignores all lines starting with a number character
"abc" ignores all lines containing exclusively "abc"
"*abc*" ignores all lines containing abc e.g. "123abc456" or "abc456"
"*^" ignores all lines starting with a capital letter
"*_*" ignores all lines containing an underscore character

Separators can be specified if the card sides are divided by a given symbol. Example:
"The dog;Der Hund" can be used if the semicolon separator is specified
A valid separators input should look like "/,;,." in this case the slash, semicolon and period symbols will be used as separators.

The line pattern is the core of the TXT reader. This tells the parser how to read the TXT file. If the TXT file looked like this:

The dog     m    Hund
The cat     f    Katze
The house   n    Haus
To go            Gehen

The first part of the line pattern should be "[1],gender?,[2];". This describes the pattern of each line, in other words it breaks down the elements that each line contains, here, [1] is the side 1 text, [2] is the side 2 text and gender is the optional gender of the nouns. The ? operator indicates that the gender field is optional and might not be in a given line. [1] and [2] fields are compulsory and must appear in the line description.

In addition to the line description, the line pattern must also include instructions indicating what the parser should do with optional variables, here the gender. Therefore the complete instruction here would look something like 

[1],gender?,[2]; gender=={"m":[2]="der "+[2], "f":[2]="die "+[2], "n":[2]="das "+[2]};

The first part is the line description, followed by the instructions for the gender optional variable. The gender variable has three possible values if it exists, these values are "m", "f" and "n", in every case, the parser is instructed to assign a new value to the field [2]. This new value is simply a constant string + the existing field.

Note that instructions can use the same value twice e.g. gender=={"m":[2]="der "+[2], "m":[1]="the "+[1]}
