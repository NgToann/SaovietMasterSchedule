   M S B A M L   `   `   `  ����  MasterSchedule1    +MasterSchedule.Views.UpperWHInventoryWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$*  $Master Schedule - Upper WH Inventory��5   	   $	��360��66   $	��640��6C    � WindowStartupLocation$ CenterScreen=�6f   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgInventory��5
         
IsReadOnly .    AutoGenerateColumns .   �� AlternationCount$ 2��5         RowHeaderWidth$ 2Q�6(      AlternatingRowBackground
 �����	   HorizontalGridLinesBrush
	 �����
   VerticalGridLinesBrush

 �����   CanUserReorderColumns .     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      -   5        Column1��,  &System.Windows.Controls.DataGridColumn   Header$ Assembly Line��5   6      
FontWeight$
 Bold"�6n      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6M   �� *AssemblyLine+6�     5      -   5        Column2��$ Upper��5   6    .  6E   �� *SewingOutput+6f     5      -   5        Column3��$ Outsole��5   6    .  6G   �� *OutsoleOutput+6i     5      -   5        	Column3_1��$ 
Socklining��5   8    .  6L   �� *SockliningInput+6p     5      -   5        Column4��$ Matching��5   6    .  6H   �� *
Matching+6e   5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      ��6#   ��  �� ElementName$ dgInventory�� �� Path$ BorderThickness.Left��� 5      ��6#   �� $ dgInventory��$ RowHeaderWidth��� 5      ��6#   �� $ Column1��$ ActualWidth��� 5      ��6#   �� $ Column2��$ ActualWidth��� 5      ��6#   �� $ Column3��$ ActualWidth��� 5      ��6#   �� $ 	Column3_1��$ ActualWidth��� 5      ��6#   �� $ Column4��$ ActualWidth��� 5       ��6#   �� $ Column5��$ ActualWidth�.+�5"      �� $��2��6   $��1,0,1,1q�6%   
���   �._�5#      �� $��1,0,0,0q�6   $��TOTAL��6-   $��Center=�6:   $
��Bold"�6U   5$      �� 5%      $��3��6   $��0,0,1,1q�6%   
���   �._�5&      �� -   5         ��Name$ lblSewingOutput��5&      $��1,0,0,0q�65   $��0��6F   $��Center=�6O   $
��Bold"�6j   5'      �� 5(      $��4��6   $��0,0,1,1q�6%   
���   �._�5)      �� -	   5        $ lblOutsoleOutput��5)      $��1,0,0,0q�66   $��Center=�6G   $��0��6b   $
��Bold"�6k   5*      �� 5+      $��5��6   $��0,0,1,1q�6%   
���   �._�5,      �� -
   5        $ lblSocklining��5,      $��1,0,0,0q�63   $��0��6D   $��Center=�6M   $
��Bold"�6h   5-      �� 5.      $��6��6   $��0,0,1,1q�6%   
���   �._�5/      �� -   5        $ lblMatching��5/      $��1,0,0,0q�61   $��0��6B   $��Center=�6K   $
��Bold"�6f   50      �� 51      $��7��6   $��0,0,1,1q�6%   
���   �._�52      �  53      �� 54      $
��auto �6+   �� 55      $��* �6+   �� 56      $
��auto �6+   .+�58      �� -   5         ��Name$ btnPrint��58   -   $��0��6   $
��Left=�6?   $��5,0,0,0q�6Z   $��  _Print Report  ��6k   {�.  6�   �� 59      -   5        $ btnMatchingWithSocklining��59   -   $��2��6   $#��  _Matching With Socklining  ��6P   {�.  6�   5:      5;      5<      5=      5>      