   M S B A M L   `   `   `  ����E  ASwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SwipeFlash
SwipeFlash  5      "    SwipeFlash.FamilyStatsWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      >d2http://schemas.microsoft.com/expression/blend/2008    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      *localclr-namespace:SwipeFlash     5        � WindowStartupLocation$  CenterOwner=�5       � WindowStyle$ 
ToolWindow=�5       � SizeToContent$ WidthAndHeight=�5	       � Title$ Stats��5      $	��200��5      $	��200��6"   .��5      ��  �� Padding$ 10q�6   ._�5   
   �� .+�5      ��    FontSizeLarge#��[  6   $��Center=�5      $��0 0 0 20q�5      ��5      �� *
FamilyName+5      �� 5      .+�5      � -   5         �Name$ barChart��5      $	��100��5      $	��300��5       � ColumnDefinitions 5      �� 5      $��* �6+   �� 5      $��* �6+   �� 5      $��* �6+   �� 5      $��* �6+   �� 5      $��* �6+   .+�5#      � $��0��6"    � Orientation$ Vertical=�5$   "    � Minimum$ 0Q�5%   "   	 � Maximum$	 1Q�6.   ���#df0000
������ $��0q�5'   "   $
��10 0q�5(   "   
 � Value
 5%   :   �� *Category1BarSize+ �� Mode$ OneWay=�5(   "   � 5*      $��1��6"   $ Vertical=�5+   "   $ 0Q�5,   "   $	 1Q�6.   ���#8f2f00
������ $��0q�5.   "   $
��10 0q�5/   "   
 5,   :   �� *Category2BarSize+$ OneWay=�5/   "   � 51      $��2��6"   $ Vertical=�52   "   $ 0Q�53   "   $	 1Q�6.   ���#efdf00
������ $��0q�55   "   $
��10 0q�56   "   
 53   :   �� *Category3BarSize+$ OneWay=�56   "   � 58      $��3��6"   $ Vertical=�59   "   $ 0Q�5:   "   $	 1Q�6.   ���#00df00
������ $��0q�5<   "   $
��10 0q�5=   "   
 5:   :   �� *Category4BarSize+$ OneWay=�5=   "   � 5?      $��4��6"   $ Vertical=�5@   "   $ 0Q�5A   "   $	 1Q�6.   ���#00af00
������ $��0q�5C   "   $
��10 0q�5D   "   
 5A   :   �� *Category5BarSize+$ OneWay=�5D   "   5E      �� 5H      	 �� X1$ 0��5I      	 �� Y1$ 0��6   	 �� X2$	 300��5J      	 �� Y2$ 0��6     ForegroundVeryDarkBrush#��[ 5K      $��1��5L      � 5O      $��0 3 0 0q�5P      ��5O      �� *ActualWidth+ �� ElementName$ barChart�� 5Q      �� 5R      $��* �6+   �� 5S      $��* �6+   �� 5T      $��* �6+   �� 5U      $��* �6+   �� 5V      $��* �6+   .+�5Z      �� $��0��6    $��Center=�5\         FontSizeSmaller#��[ 5]       ��5[       �� *Category1Count+5]       �� 5_      $��1��6    $��Center=�5a       #��[ 5b       ��5`       �� *Category2Count+5b       �� 5d      $��2��6    $��Center=�5f       #��[ 5g       ��5e       �� *Category3Count+5g       �� 5i      $��3��6    $��Center=�5k       #��[ 5l       ��5j       �� *Category4Count+5l       �� 5n      $��4��6    $��Center=�5p       #��[ 5q       ��5o       �� *Category5Count+5q       5s      5u      �� 5x       �� Orientation$ 
Horizontal=�6   $��0 10 0 0q�5y      .+�5z      �� $��Last studied ��6   �� 5{      ��6   �� *LastStudiedDisplayString+5|      �� 5�      $��Close��6   $��40��5�      $��Right=�5�        FontSizeRegular#��[ 5�      $��0q�5�      $	��120��5�      ��5�      �� *CloseButtonCommand+5�      5�      5�      