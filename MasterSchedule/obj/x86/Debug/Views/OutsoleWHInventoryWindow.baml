   M S B A M L   `   `   `  ����  MasterSchedule3    -MasterSchedule.Views.OutsoleWHInventoryWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $��13(�5   @     � Title$,  &Master Schedule - Outsole WH Inventory��5   	   $	��360��68   $	��640��6E    � WindowStartupLocation$ CenterScreen=�6h   .��5      � $��3q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5	      $
��Auto �6   .+�5   
   � $��0,0,0,5q�6    � ColumnDefinitions 5      �� 5      $
��auto �6#   �� 5      .+�5      ��    GroupBoxTemplate#���   6   ��5      �� 5       �� Padding$ 4q�6     Background1#���  6)   ._�5      �� $��Inventory By��6$   5      .��5      �  5      �� 5      �� 5      $��10 �6+   �� 5      .+�5      	� -   5         	�Name$ radByOutsoleLine��5   "   $��Outsole Line��6<   $��Center=�6S   $��Center=�6u    	� 	GroupName$	 ByX��6�    � 	Resources 5       �� 
TargetType	 �� Triggers
 P� Property P� Value �� Property �� Value��%K  '	�       �� 5      " 	�6$   	 5   "   P� 5    &   
 �@d�	IsChecked$
 TrueY�6O   . �5!   *   ��  � ��
 ��  �6G   �� 5"   *    � ��$
 Bold"�6G   5#   '   P� 5$   &   
 �@d�	IsChecked$ FalseY�6O   . �5%   *   ��  � ��
 �   �6G   �� 5&   *    � ��$ Normal"�6G   5'   '   5)      5*      5+      	� 5,      -   5        $ radByOutsoleCode��5,   2   $��2��6"   $��O/S Code��6L   $��Center=�6_   $��Center=�6�    d� 	IsChecked$
 TrueY�6�   $	 ByX��6�    5-      ��%K  '	�       �� 5.      " 	�6$   	 5/   "   P� 50   &   
 �@d�	IsChecked$
 TrueY�6O   . �51   *   ��  � ��
 ��  �6G   �� 52   *    � ��$
 Bold"�6G   53   '   P� 54   &   
 �@d�	IsChecked$ FalseY�6O   . �55   *   ��  � ��
 �   �6G   �� 56   *    � ��$ Normal"�6G   57   '   59      5:      5;      5<      5=      5>      &   System.Windows.Controls.DataGrid  5?   
   -   5          Name$ dgInventory��5?   !   $��1��6      RowHeaderWidth$ 2Q�66      
IsReadOnly .    AutoGenerateColumns .   �� AlternationCount$ 2��5A      $��12(�6(      AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   CanUserReorderColumns .     Columns 5D      0  *System.Windows.Controls.DataGridTextColumn  5E      -   5        Column1��,  &System.Windows.Controls.DataGridColumn   Header$ O/S Code��5E   6      
FontWeight$
 Bold"�6h      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6H   �� *OutsoleCode+6z     5F      -   5        Column2��$ 	Total Qty��5F   6    .  6I   �� *
Quantity+6f     5G      -   5        Column3��$ Matching��5G   6    .  6H   �� *
Matching+6e     5H      -   5        Column4��$ Finished Outsole��5H   6    .  6P   �� *FinishedOutsoleQuantity+6|   5J      � 5K   
   $��2��6    5L      �� 5M      ��6#   ��  �� ElementName$ dgInventory�� �� Path$ BorderThickness.Left��� 5N      ��6#   �� $ dgInventory��$ RowHeaderWidth��� 5O      ��6#   �� $ Column1��$ ActualWidth��� 5P      ��6#   �� $ Column2��$ ActualWidth��� 5Q      ��6#   �� $ Column3��$ ActualWidth��� 5R      ��6#   �� $ Column4��$ ActualWidth�.+�5T      �� $��2��6   $��1,0,1,1q�6%   
���   �._�5U      �� $��1,0,0,0q�6   $��TOTAL��6-   $��SemiBold"�6:   5V      �� 5W      $��3��6   $��0,0,1,1q�6%   
���   �._�5X      �� -	   5         ��Name$ lblTotalQTy��5X      $��1,0,0,0q�61   $��0��6B   $��SemiBold"�6K   5Y      �� 5Z      $��4��6   $��0,0,1,1q�6%   
���   �._�5[      �� -
   5        $ lblMatching��5[      $��1,0,0,0q�61   $��0��6B   $��SemiBold"�6K   5\      �� 5]      $��5��6   $��0,0,1,1q�6%   
���   �._�5^      �� -   5        $ lblFinishedOutsole��5^      $��1,0,0,0q�68   $��0��6I   $��SemiBold"�6R   5_      5`      5a      5b      