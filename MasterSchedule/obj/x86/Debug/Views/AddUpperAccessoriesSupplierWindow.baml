   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.2.8.9, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views  5   	   <    6MasterSchedule.Views.AddUpperAccessoriesSupplierWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5        $��13(�5        � WindowStartupLocation$  CenterScreen=�6?    � Title$6 0Master Schedule - Add Upper Accessories Supplier��5   	   $	��450��6B   $	��800��6O   .��5	      � $��2q�6    � ColumnDefinitions 5
   
   �� 5      $��* �6   �� 5      $��10 �6   �� 5      $��2* �6   .+�5   
   ��    GroupBoxTemplate_HeaderAuto#���   6   ��5      �� 5      -   5         ��Name$ brAddNew��5        GroupBackground#���  6+     BorderStyle#���  6f   ._�5      �� $��Add a new Supplier��6    5      .��5      �  � RowDefinitions 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5      �� $��Supplier Name��6   �� 5      -   5         ��Name$ txtSupplierName��5   '   $��1��6   $��2,0,0,0q�6@   $��Center=�6R   ��6t   ��  �� ElementName$ btnAdd�� �� Path$ ActualHeight��� 5       $��2��6   $��0,10,0,0q�6)   $��Accessories Name��6;   �� 5!      -   5        $ txtAccessoriesName��5!   '   $��3��6   $��2,0,0,0q�6C   $��Center=�6U   ��6w   �� $ btnAdd��$ ActualHeight�� 5#      $��4��6   $��0,10,0,0q�6$    5$      �� 5%      �� 5&      $
��Auto �6+   .+�5(      �� -   5         ��Name$ btnAdd��5(        ButtonTemplate#���  6-   $��  Add  ��6e   $��1��6w   	 � Cursor$
	 Hand��6�   6�   5)      5*      5+      � 5,   
   $��2��6    5-      �� 5.      $
��auto �6    �� 5/      .+�51      �� $)��#List of Upper Accessories Suppliers��6   $��Center=�6C   $��Center=�6`   $��Italic$�6{   ��6�   �� $ brAddNew��$ ActualHeight�&   System.Windows.Controls.DataGrid  52      -   5        
  Name$
 dgSuppliers��52   $   $��1��6      AutoGenerateColumns .     
IsReadOnly .    AlternatingRowBackground
 ����� �� AlternationCount$ 2��53   8      RowHeaderWidth$	 NaNQ�6q      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 ����� � 	Resources 55      *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value��%�   '        �� 56      "  6    57      P� 58       �@ 
IsSelected . . �59   "   ��  � ��
 ��iA�6?   �� 5:   "    � ��
 �����6?   5;      5=      5>         Columns 5?      0  *System.Windows.Controls.DataGridTextColumn  5@      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6)      Header$ Accessories Name��66   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6P   �� *ProvideAccessories+  5A      $
 Auto 6)   $ Supplier��66    6H   �� *Name+5C      5D      5E      5F      