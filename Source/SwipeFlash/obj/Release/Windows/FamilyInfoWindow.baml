   M S B A M L   `   `   `  ���� E  ASwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SwipeFlash
SwipeFlash  !    SwipeFlash.FamilyInfoWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:SwipeFlash       � WindowStartupLocation$  CenterOwner=� � WindowStyle$ 
ToolWindow=� � Title$ Flashcard family�� � SizeToContent$ WidthAndHeight=�.����  �� Padding$ 10q�._��� .+��� $��Flashcard family name:��$��0 0 0 5q�   FontSizeLarge#��[  �� #��[  ���� *
FamilyName+�  � ColumnDefinitions �� $
��Auto ��� $��30 ��� $
��Auto �.+��� $��0��.+��� $��Side 1 logo:��$��0 0 0 5q�#��[  �� -   $��logo1��$	��0 4q�$��50��$
��Left=����� *FamilyLogo1+�� $��Side 1 category��$��0 0 0 5q�#��[  �� $	��0 4q�$	��140��$
��Left=� �� 
IsEditable . ���� *
Categories+ �� Text �� *FamilyCategory1+���� .R��� ���� �� $��Side 1 articles:��$��0 0 0 5q�#��[  �� $
��Left=�$	��140��#��[  ���� *Category1Articles+{��� *IsFamilyDataInputEnabled+�� $��2��$��Right=�.+��� $��Side 2 logo:��$��0 0 0 5q�#��[  �� $	��0 4q�$��50��$
��Left=����� *FamilyLogo2+�� $��Side 2 category��$��0 0 0 5q�#��[  �� $	��0 4q�$	��140��$
��Left=� �� DisplayMemberPath$  �� . ���� *
Categories+ �� *FamilyCategory2+���� .R��� ���� �� $��Side 2 articles:��$��0 0 0 5q�#��[  �� $
��Left=�$	��140��#��[  ���� *Category2Articles+{��� *IsFamilyDataInputEnabled+�� 	 �� Orientation$	 
Horizontal=�$��0 5 0 0q�.+��� $��Center=�$��0q�
 d� 	IsChecked
 �� *AreIllustrationsEnabled+�� $��Enable illustrations��$
��10 0q�$��Center=�#��[  � $��0 0 0 10q�.+��� $��5 0 0 0q�$��Center=�#��[  ���� *CardCountDisplayText+ �� FallbackValue$ 
4000 cards���� $��Export Flashcards...��$��Right=�$	��150��$��40��$��7q�  FontSizeRegular#��[ ���� *ExportFamilyCommand+�� $	 
Horizontal=�$��Right=�$��40��.+��� $��Cancel��$��0q�#��[ $��0q�$	��120������ *CancelButtonCommand+�� $��OK��$��10 0 0 0q� �� 	IsDefault . $��0q�$	��120��#��[ ���� *OKButtonCommand+