   M S B A M L   `   `   `  ����E  ASwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SwipeFlash
SwipeFlash  5      J FSwipeFlash.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.CoreSwipeFlash.Core 5      /    )SwipeFlash.FlashcardFamilyListItemControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      *localclr-namespace:SwipeFlash     5      Gcore6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.Core     5      .��5
      �   � ColumnDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��10q�6.   $��0��6:   .+�5      �� $��0 0 10 0q�6    d� 	IsChecked 6)   �� *IsFamilyEnabled+�� 5      $��Center=�6   $
��10 0q�64   .+�5      �� $��Center=�68   ��6   �� *
FamilyName+68   �� 5      $ 
Horizontal=�6   .+�5      ��    FontSizeSmall#��[  6;   $��Center=�6e   ��6    �� *	CardCount+6e   �� 5      $�� Cards��6    #��[  6.   $��Center=�6X   5      5      5      �� 5   
   $ 
Horizontal=�6   $��2��5      .+�5      ��   BorderlessIconButton#��[ 6     FontAwesome_Eye#��[ 5       ��5!      �� *OpenFamilyInfoCommand+�� 5#      #��[ 6     FontAwesome_Stats#��[ 5$      ��5%      �� *OpenFamilyStatsCommand+�� 5'      #��[ 6     FontAwesome_Trash#��[ 5(      $��10q�5)      ��5*      �� *DeleteFamilyCommand+5+      5.      5/      