   M S B A M L   `   `   `  ����E  ASwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SwipeFlash
SwipeFlash  5      !    SwipeFlash.FamilyInfoWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      *localclr-namespace:SwipeFlash     5        � WindowStartupLocation$  CenterOwner=�5       � WindowStyle$ 
ToolWindow=�5	       � Title$ Flashcard family��5
       � SizeToContent$ WidthAndHeight=�5      .��5      ��  �� Padding$ 10q�6   ._�5   
   �� .+�5      �� $��Flashcard family name:��6   $��0 0 0 5q�5         FontSizeLarge#��[  5      �� 5      #��[  6   ��5      �� *
FamilyName+� 5       � ColumnDefinitions 5      �� 5      $
��Auto �6'   �� 5      $��30 �6'   �� 5      $
��Auto �6'   .+�5      �� $��0��6   .+�5       �� $��Side 1 logo:��6    $��0 0 0 5q�5!      #��[  5"      �� 5$      -   5        $��logo1��5$      $	��0 4q�5%      $��50��5&      $
��Left=�5'      ��5(      �� *FamilyLogo1+�� 5*      $��Side 1 category��6    $��0 0 0 5q�5+       #��[  5,       �� 5.      $	��0 4q�6   $	��140��5/      $
��Left=�50       �� 
IsEditable . ��51      �� *
Categories+ �� Text 53      �� *FamilyCategory1+��54      �� 55      .R�56   "   �� ��6,   �� 57      59      �� 5;      $��Side 1 articles:��6    $��0 0 0 5q�5<       #��[  5=       �� 5?      $
��Left=�6   $	��140��5A      #��[  5B      ��5@      �� *Category1Articles+{�5C      �� *IsFamilyDataInputEnabled+5D      �� 5F      $��2��6   $��Right=�6-   .+�5G      �� $��Side 2 logo:��6    $��0 0 0 5q�5H       #��[  5I       �� 5K      $	��0 4q�6   $��50��5L      $
��Left=�5M      ��5N      �� *FamilyLogo2+�� 5P      $��Side 2 category��6    $��0 0 0 5q�5Q       #��[  5R       �� 5T      $	��0 4q�6   $	��140��5U      $
��Left=�5V       �� DisplayMemberPath$  ��5X       . ��5W      �� *
Categories+ 5Z      �� *FamilyCategory2+��5[      �� 5\      .R�5]   "   �� ��6,   �� 5^      5`      �� 5b      $��Side 2 articles:��6    $��0 0 0 5q�5c       #��[  5d       �� 5f      $
��Left=�6   $	��140��5h      #��[  5i      ��5g      �� *Category2Articles+{�5j      �� *IsFamilyDataInputEnabled+5k      5n      �� 5p      	 �� Orientation$	 
Horizontal=�6   $��0 5 0 0q�62   .+�5q      �� $��Center=�6   $��0q�5r      
 d� 	IsChecked
 5s      �� *AreIllustrationsEnabled+�� 5t      $��Enable illustrations��6   $
��10 0q�5u      $��Center=�5v      #��[  5w      5x      � 5z      $��0 0 0 10q�6   .+�5{      �� $��5 0 0 0q�5|      $��Center=�5}      #��[  5~      ��5{      �� *CardCountDisplayText+ �� FallbackValue$ 
4000 cards��5~      �� 5�      $��Export Flashcards...��6   $��Right=�5�      $	��150��5�      $��40��5�      $��7q�5�        FontSizeRegular#��[ 5�      ��5�      �� *ExportFamilyCommand+5�      �� 5�      $	 
Horizontal=�6   $��Right=�5�      $��40��5�      .+�5�      �� $��Cancel��6   $��0q�5�      #��[ 5�      $��0q�5�      $	��120��5�      ��5�      �� *CancelButtonCommand+�� 5�      $��OK��6   $��10 0 0 0q�5�       �� 	IsDefault . $��0q�5�      $	��120��5�      #��[ 5�      ��5�      �� *OKButtonCommand+5�      5�      5�      5�      