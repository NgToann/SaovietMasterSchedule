   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.3.0.4, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views  5   	   1    +MasterSchedule.Views.ImportSizeRunNewWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   1    � Title$4 .Master Schedule - Import SizeRun and OrderList��5   	   $	��450��6@   $
��1000��6M   .��5	      � $��2q�6    � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��5 �6   �� 5      $��5* �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ 	dgSizeRun��5         AutoGenerateColumns .   �� AlternationCount$ 2��6C      AlternatingRowBackground
 �����   CanUserSortColumns . 	   CanUserAddRows	 .  
   SelectionUnit$
 FullRow=�5   ,      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   
IsReadOnly .    RowHeaderWidth$	 NaNQ�5   '   $��Times New Roman)�6<   $��14(�6Y      FrozenColumnCount$ 1��6g      	CellStyle 5      �� 5       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5      P� 5       P� Property �@ 
IsSelected P� Value . . �5      ��  �� Property � �� �� Value
 �   �6;   �� 5       � ��
 ��� �6;   �� 5       � ��$ 0,0,0,0q�6@   5      5          ColumnHeaderStyle 5"      �� 5#      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5$      ��  � ��$ Center=�6C   �� 5%       � ��$ 14(�61   �� 5&       � ��
 �����63   �� 5'       � ��
 �����64   �� 5(       � ��$ 0.5,0.5,0.5,0q�68   �� 5)       � ��$ 4q�60   5*         Columns 5,      4  .System.Windows.Controls.DataGridTemplateColumn  5-      ,  &System.Windows.Controls.DataGridColumn   Header$ Prod. No��6)      SortMemberPath$ 	ProductNo��6;      CellTemplate 5.      �� 5/      .R�50      �� $��3,2,3,2q�6D   ��6(   �� *	ProductNo+6D   51      53        54      $ Order
 Size��6)   $ SizeNo��6B    55      �� 56      .R�57      �� $��3,2,3,2q�6A   ��6(   �� *SizeNo+6A   58      5:        5;      $ Outsole
   Size��6)   $ OutsoleSize��6F    5<      �� 5=      .R�5>      �� $��3,2,3,2q�6F   ��6(   �� *OutsoleSize+6F   5?      5A        5B      $ Midsole
   Size��6)   $ MidsoleSize��6F    5C      �� 5D      .R�5E      �� $��3,2,3,2q�6F   ��6(   �� *MidsoleSize+6F   5F      5H        5I      $ 	Last
Size��6)   $ LastSize��6@    5J      �� 5K      .R�5L      �� $��3,2,3,2q�6C   ��6(   �� *
LastSize+6C   5M      5O        5P      $ Quantity��6)   $ Quantity��6;    5Q      �� 5R      .R�5S      �� $��3,2,3,2q�6C   ��6(   �� *
Quantity+6C   5T      5V      5X        5Z   
   -   5        $ dgOrders��5Z   #   $��2��6    .  $ 2��6Q   
 ����� . 	 .  $
 FullRow=�5[   ,   
 �����
 ����� . $	 NaNQ�5\   '   $��Times New Roman)�6<   $��14(�6Y   $ 1��6g    5]      �� 5^      "  6    5_      P� 5`       �@ 
IsSelected . . �5a      ��  � ��
 �   �6;   �� 5b       � ��
 ��� �6;   �� 5c       � ��$ 0,0,0,0q�6@   5d      5f       5h      �� 5i      "  6   .�5j      ��  � ��$ Center=�6C   �� 5k       � ��$ 14(�61   �� 5l       � ��
 �����63   �� 5m       � ��
 �����64   �� 5n       � ��$ 0.5,0.5,0.5,0q�68   �� 5o       � ��$ 4q�60   5p       5r        5s      $ 	Prod. No.��6)   $ 	ProductNo��6<    5t      �� 5u      .R�5v      �� $��3,2,3,2q�6D   ��6(   �� *	ProductNo+6D   5w      5y        5z      $ U.Customer
     Code��6)   $ UCustomerCode��6K    5{      �� 5|      .R�5}      �� $��3,2,3,2q�6H   ��6(   �� *UCustomerCode+6H   5~      5�        5�      $ GTN
PONo��6)   $ GTNPONo��6?    5�      �� 5�      .R�5�      �� $��3,2,3,2q�6B   ��6(   �� *	GTNPONo+6B   5�      5�        5�      $ E.F.D��6)   $	 ETD��68    5�      �� 5�      .R�5�      �� $��3,2,3,2q�6W   ��6(   �� *ETD+ �� StringFormat$ 
MM/dd/yyyy��6W   5�      5�        5�      $ 
Article No��6)   $ 	ArticleNo��6=    5�      �� 5�      .R�5�      �� $��3,2,3,2q�6D   ��6(   �� *	ArticleNo+6D   5�      5�        5�      $ 	Shoe Name��6)   $ ShoeName��6<    5�      �� 5�      .R�5�      �� $��3,2,3,2q�6C   ��6(   �� *
ShoeName+6C   5�      5�        5�      $ Quantity��6)   $ Quantity��6;    5�      �� 5�      .R�5�      �� $��3,2,3,2q�6C   ��6(   �� *
Quantity+6C   5�      5�        5�      $ 
Pattern No��6)   $ 	PatternNo��6=    5�      �� 5�      .R�5�      �� $��3,2,3,2q�6D   ��6(   �� *	PatternNo+6D   5�      5�        5�      $ Midsole Code��6)   $ MidsoleCode��6?    5�      �� 5�      .R�5�      �� $��3,2,3,2q�6F   ��6(   �� *MidsoleCode+6F   5�      5�        5�      $ Outsole Code��6)   $ OutsoleCode��6?    5�      �� 5�      .R�5�      �� $��3,2,3,2q�6F   ��6(   �� *OutsoleCode+6F   5�      5�        5�      $ 	Last Code��6)   $ LastCode��6<    5�      �� 5�      .R�5�      �� $��3,2,3,2q�6C   ��6(   �� *
LastCode+6C   5�      5�        5�      $ Country��6)   $ Country��6:    5�      �� 5�      .R�5�      �� $��3,2,3,2q�6B   ��6(   �� *	Country+6B   5�      5�      5�      � 5�   
   $��2��6   $��3��6   $��0,5,0,0q�60   .+�5�      �� .@�5�      �� $��Processing:��6   � 5�      -   5         �Name$ progressBar��5�      $	��100��63   $��15��6?    � Minimum$ 0Q�6K    � Value$ 0Q�6W   �� 5�      -   5          ��Name$  	lblStatus��5�      $�� ��6/   $
��Bold"�67   5�      � 5�      $��Right=�6    5�      �� 5�      $
��auto �6'   �� 5�      $��10 �6'   �� 5�      $
��auto �6'   .+�5�      �� -   5        ! ��Name$! 	btnImport��5�         ButtonTemplate#���   6,   $��Right=�6d   $��  Import SizeRun  ��6�   " � ToolTip$&"  Click to Import to Orders Table.��6�   {�.  6�   �� 5�      -   5        $! btnImportOrders��5�      $��2��62   $��  Import Orders  ��6B   #���   6^   $&"  Click to Import to Orders Table.��6�   {�.  6�   5�      5�      5�      5�      