   M S B A M L   `   `   `  ����  MasterSchedule3    -MasterSchedule.Views.UpdateSpecifyOrderWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $��13(�5   @     � Title$,  &Master Schedule - Update Specify Order��5   	   $	��450��68   $	��800��6E    � WindowStartupLocation$ CenterScreen=�6h   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   .+�5   
   ��    GroupBoxTemplate#���   6   ��5      �� 5       �� Padding$ 4q�6     Background1#���  6%   ._�5      �� $��Update What ?��6    5      .��5      � -   5         �Name$ gridUpdateWhat��5      $��0,4,0,4q�6+    � ColumnDefinitions 5      �� 5      �� 5      �� 5      �� 5      �� 5      5      5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgOrders��5   1   $��1��6   $��0,2,0,0q�6       AutoGenerateColumns .   �� AlternationCount$ 2��6_   	   AlternatingRowBackground
	 �����
   CanUserSortColumns
 .    CanUserAddRows .     SelectionUnit$ FullRow=�5   ,      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   RowHeaderWidth$	 NaNQ�5      $��Times New Roman)�6*   $��14(�6G      FrozenColumnCount$ 1��6U      	CellStyle 5      �� 5        �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5!      P� 5"       P� Property �@ 
IsSelected P� Value . . �5#      ��  �� Property � �� �� Value
 �   �6;   �� 5$       � ��
 ��� �6;   �� 5%       � ��$ 0,0,0,0q�6@   5&      5(         ColumnHeaderStyle 5*      �� 5+      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5,      ��  � ��$ Center=�6C   �� 5-       � ��$ 14(�61   �� 5.       � ��
 �����63   �� 5/       � ��
 �����64   �� 50       � ��$ 0.5,0.5,0.5,0q�68   �� 51       � ��$ 4q�60   52         Columns 54      4  .System.Windows.Controls.DataGridTemplateColumn  55      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Prod. No.��6)      SortMemberPath$ 	ProductNo��6<      CellTemplate 56      �� 57      .R�58      �� $��3,2,3,2q�6D   ��6(   �� *	ProductNo+6D   59      5;        5<      $ U.Customer
     Code��6)   $ UCustomerCode��6K    5=      �� 5>      .R�5?      �� $��3,2,3,2q�6H   ��6(   �� *UCustomerCode+6H   5@      5B        5C      $ GTN
PONo��6)   $ GTNPONo��6?    5D      �� 5E      .R�5F      �� $��3,2,3,2q�6B   ��6(   �� *	GTNPONo+6B   5G      5I        5J      $ E.F.D��6)   $	 ETD��68    5K      �� 5L      .R�5M      �� $��3,2,3,2q�6W   ��6(   �� *ETD+ �� StringFormat$ 
MM/dd/yyyy��6W   5N      5P        5Q      $ 
Article No��6)   $ 	ArticleNo��6=    5R      �� 5S      .R�5T      �� $��3,2,3,2q�6D   ��6(   �� *	ArticleNo+6D   5U      5W        5X      $ 	Shoe Name��6)   $ ShoeName��6<    5Y      �� 5Z      .R�5[      �� $��3,2,3,2q�6C   ��6(   �� *
ShoeName+6C   5\      5^        5_      $ Quantity��6)   $ Quantity��6;    5`      �� 5a      .R�5b      �� $��3,2,3,2q�6C   ��6(   �� *
Quantity+6C   5c      5e        5f      $ 
Pattern No��6)   $ 	PatternNo��6=    5g      �� 5h      .R�5i      �� $��3,2,3,2q�6D   ��6(   �� *	PatternNo+6D   5j      5l        5m      $ Midsole Code��6)   $ MidsoleCode��6?    5n      �� 5o      .R�5p      �� $��3,2,3,2q�6F   ��6(   �� *MidsoleCode+6F   5q      5s        5t      $ Outsole Code��6)   $ OutsoleCode��6?    5u      �� 5v      .R�5w      �� $��3,2,3,2q�6F   ��6(   �� *OutsoleCode+6F   5x      5z        5{      $ 	Last Code��6)   $ LastCode��6<    5|      �� 5}      .R�5~      �� $��3,2,3,2q�6C   ��6(   �� *
LastCode+6C   5      5�        5�      $ Country��6)   $ Country��6:    5�      �� 5�      .R�5�      �� $��3,2,3,2q�6B   ��6(   �� *	Country+6B   5�      5�      5�      � 5�   
   $��3��6   $��0,5,0,0q�6   .+�5�      �� .@�5�      �� $��Processing:��6   � 5�      -   5         �Name$ progressBar��5�      $	��100��63   $��15��6?    � Minimum$ 0Q�6K     � Value$  0Q�6W   �� 5�      -   5        ! ��Name$! 	lblStatus��5�      $�� ��6/   $
��Bold"�67   5�      �� 5�      -   5        " ��Name$" 	btnImport��5�      $��2��6(     ButtonTemplate#���  65   $��Right=�6m   $��
  Update  ��6�   # � ToolTip$&#  Click to Update to Orders Table.��6�   {�.  6�   5�      5�      5�      