   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.3.0.4, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views  5   	   9    3MasterSchedule.Views.ImportLaminationMaterialWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   $��13(�6:     � WindowStartupLocation$  CenterScreen=�5       � Title$2 ,Master Schedule - Import Lamination Material��5   	   $	��450��6>   $	��800��6K   .��5	      � $��2q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   .+�5   
   �  � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      .+�5      �  5      �� 5      $��* �6$   �� 5      $
��Auto �6$   .+�5      �� -   5         ��Name$ btnOpenExcel��5      $��1��6/   $��  Open Excel File  ��6<   6Z   5      �� 5      $��1��6   $��Right=�6)    �� Orientation$ 
Horizontal=�6E   .+�5      �� -   5         ��Name$ 
txtOrderNo��5      $	��120��6.   $	��180��6=   
���   �$��Center=�6`   $��2,1,0,1q�6�   $��14(�6�    � ToolTip$ Input OrderNo��6�   �� 5      -   5        $ 	btnFilter��5         ButtonNoneTemplate#���   6,    � Cursor$
 Hand��6h   ��5      �� 5       $��16��6    $��16��6+   $��/Images/search.png��67   5"      5#      5$      &   System.Windows.Controls.DataGrid  5%   
   -   5        	  Name$	 dgLaminationMaterial��5%      $��1��61   $��0,2,0,0q�6>   
   AutoGenerateColumns
 .     
IsReadOnly .    AlternatingRowBackground
 ����� �� AlternationCount$ 2��5&   8      RowHeaderWidth$	 NaNQ�6z      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   ColumnHeaderStyle 5(      �� 5)       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5*      ��  �� Property � �� �� Value$ Center=�6C   �� 5+       � ��$ 14(�61   �� 5,       � ��
 �����63   �� 5-       � ��
 �   �63   �� 5.       � ��
 �����64   �� 5/       � ��$ 0.5,0.5,0.5,0.5q�68   �� 50       � ��$ 5,4,5,4q�60   51       � 	Resources 53      *  $System.Windows.Controls.DataGridCell �� Triggers P� Property P� Value��%�   '        �� 54      "  6    55      P� 56       �@ 
IsSelected . . �57      ��  � ��
 ��iA�6;   �� 58       � ��
 �����6;   59      5;      5<         Columns 5=      0  *System.Windows.Controls.DataGridTextColumn  5>      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6%      Header$ OrderId��62   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6C   �� *	OrderNoId+  5?      $
 Auto 6%   $ OrderNo��62    6C   �� *	OrderNo+  5@      $
 Auto 6%   $ 	ArticleNo��62    6E   �� *	ArticleNo+  5A      $
 Auto 6%   $ ShoeName��62    6D   �� *
ShoeName+  5B      $
 Auto 6%   $ 	PatternNo��62    6E   �� *	PatternNo+  5C      $
 Auto 6%   $ Position��62    6D   �� *
Position+  5D      $
 Auto 6%   $
 Part��62    6@   �� *MaterialPart+  5E      $
 Auto 6%   $ Material
Description��62    6T   �� *MaterialName+  5F      $
 Auto 6%   $
 Unit��62    6@   �� *Unit+  5G      $
 Auto 6%   $ PO
Quantity��62    6K   �� *
POQuantity+  5H      $
 Auto 6%   $ Send
Quantity��62    6M   �� *SendQuantity+  5I      $
 Auto 6%   $ PO List��62    6C   �� *ProductNoList+  5J      $
 Auto 6%   $ PurchaseDate��62    6H   �� *PurchaseDate+  5K      $
 Auto 6%   $ DeliveryDate��62    6H   �� *DeliveryDate+  5L      $
 Auto 6%   $ SupplierName��62    6H   �� *SupplierName+  5M      $
 Auto 6%   $ Remarks��62    6C   �� *	Remarks+5O      � 5P   
   $��2��6   $��0,2,0,0q�6    5Q      �� 5R      �� 5S      $
��Auto �6#   .+�5U      �� $ 
Horizontal=�6   $��Bottom=�62   .+�5V      � -   5         �Name$ 	prgStatus��5V      $	��100��61   $��18��6=   �� 5W      -   5         ��Name$ 	txtStatus��5W      $��10,0,0,0q�6/   $��Center=�6A   
����  �$��Italic$�6n   5X      �� 5Y      -   5        $ 	btnImport��5Y        ButtonTemplate#���  6(   $��
  Import  ��6`   $��1��6u   6�   5Z      5[      5\      