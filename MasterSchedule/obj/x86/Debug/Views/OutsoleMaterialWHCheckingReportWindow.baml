   M S B A M L   `   `   `  ����g  cMicrosoft.ReportViewer.WinForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3atSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinFormsMicrosoft.Reporting.WinForms  5   	   I EMasterSchedule, Version=1.2.8.9, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views 5   	   @   :MasterSchedule.Views.OutsoleMaterialWHCheckingReportWindow   5   	   [rpvSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinForms  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5        $��13(�5   :     � WindowState$  	Maximized=�5       � WindowStartupLocation$ CenterScreen=�62    � Title$&  Outsole Material WH Check Report��5	   	   $	��450��62   $	��800��6?   .��5
      � $��2q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      .+�5   
   �� $��0,0,0,2q�6      GroupBoxTemplate#���   6$   ��5      �� 5        GroupBackground#���  6     BorderStyle#���  6T   ._�5      �� $��Searching Information��6    
���   � �� Padding$ 	10,0,10,0q�6P   5      .��5      9�  9� 
IsExpanded . $��RightToLeft=�6)   .��5      � $��LeftToRight=�6    5      �� 5      $
��Auto �6(   �� 5      $��5 �6(   �� 5      $
��Auto �6(   �� 5      $��10 �6(   �� 5       $
��Auto �6(   �� 5!      $��10 �6(   �� 5"      $
��Auto �6(    5$      �� 5%      $
��Auto �6+   �� 5&      $��20 �6+   �� 5'      $
��Auto �6+   �� 5(      $��20 �6+   �� 5)      $
��Auto �6+   .+�5+      	� -   5         	�Name$ radPO��5+   "   $��
Product No��61   $��Center=�6F   $��Bottom=�6h    	� 	GroupName$ 
SearchWhat��6�   	 d� 	IsChecked$
	 TrueY�6�   6�   �� 5,      -   5        
 ��Name$
 txtPoSearch��5,   .   $��2��6   $��15(�6C   $��2,1,0,1q�6Q   $��Center=�6c   
���   ���6�   ��  �� ElementName$ dpFrom�� �� Path$ ActualHeight�{�6�   �� $ radPO��$ 	IsChecked�6
  	� 5-      -   5        $ radDateTime��5-   /   $��2��6"   $��
Check Date��6D   $��Center=�6Y   $��Bottom=�6{   $ 
SearchWhat��6�   6�   �� 5.      $��2��6!   $��2��6.    �� Orientation$ 
Horizontal=�6>   .+�5/      �� $
��From��6$   $��Bottom=�60   $��0,0,2,0q�6K   (  "System.Windows.Controls.DatePicker  50      -   5          Name$ dpFrom��50   %   {�65   �� $ radDateTime��$ 	IsChecked��� 51      $��to��6$   $��Bottom=�6.   $��	10,0,10,0q�6I     52      -   5        $
 dpTo��52   %   {�63   �� $ radDateTime��$ 	IsChecked�53      �� 55      $��	PO Status��6    $��4��61   $��Italic$�6>   
����  �6Q   � 56      $��4��6   $��2��6(    57      �� 58      �� 59      �� 5:      .+�5<      	� -   5        $ radPOAll��5<   &   $	��All��6U   $��Center=�6i   $ POFinishOrNot��6�   $
	 TrueY�6�   6�   	� 5=      -   5        $ radPOFinished��5=   &   $��1��6=   $��Finished��6U   $��Center=�6i   $ POFinishOrNot��6�   6�   	� 5>      -	   5        $ radPONotYetFinished��5>   &   $��2��6C   $	��Not��6U   $��Center=�6i   $ POFinishOrNot��6�   6�   5?      �� 5A      $��Supplier��6    $��6��60   �� 5B      -
   5         ��Name$ cbSuppliers��5B       �� DisplayMemberPath$
 Name��64    �� SelectedValue$ OutsoleSupplierId��6M   $��6��6o   $��2��6|   $��Center=�6�   6�   �� 5C      -   5         ��Name$ 	btnSearch��5C   >   $��4��6   $��7��6-   $��  _Search  ��6Q     ButtonTemplate#���  6g    �� 	IsDefault . 6�   5D      5E      5F      �� 5G   
   $��1��6   $��2��6"   .@�5H      �� ��5I      �� 5J      $��Summary Report��6    $��SemiBold"�66   
���   �$ 3q�6_   .��5L      7  1System.Windows.Forms.Integration.WindowsFormsHost     Child. 5M      /   )Microsoft.Reporting.WinForms.ReportViewer  -   5        reportViewer��   ShowFindControls .  5M   =   5N      5O      �� 5P      ��5Q      �� 5R      $��Reviser Incentive Report��6    $��SemiBold"�6@   
����  �$ 3q�6h   .��5T        . 5U        -   5         reportViewerReviserIncentive�� . 5U   M   5V      5W      �� 5X      ��5Y      �� 5Z      $��Reject Detail Report��6    $��SemiBold"�6<   
���  ��$ 3q�6c   .��5\        $��1��6#   . 5]        -   5        reportViewerRejectDetail�� . 5]   I   5^      5_      �� 5`      ��5a      �� 5b      $ 
Horizontal=�6!   .+�5c      �� $��WH Inventory��6$   $��SemiBold"�68   
�����_�$ 3q�6e   �� 5d      -   5        $ btnRefreshWHInventory��5d   !     ButtonNoneTemplate#���  6@    � Cursor$
 Hand��6|   ��5e      �� 5f   "   $��16��6(   $��16��63   $��/Images/mini_refresh.png��6?   5h      5i      ��5l      � 5m       5n      �� 5o      $
��auto �6/   �� 5p      $��5 �6/   �� 5q      $��* �6/   .+�5s      &   System.Windows.Controls.DataGrid  -   5          Name$ dgInventoryByOutsole��5s   #      AutoGenerateColumns .   �� AlternationCount$ 2��6]   $��Arial)�6r   $��13(�6�      CanUserAddRows .     SelectionUnit$ FullRow=�5t   2      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   RowHeaderWidth$	 NaNQ�6�      
IsReadOnly .     ColumnHeaderStyle  5v      �� 5w   "   ! �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"!  6(   .�5x   &   �� " �� Property" � ��# �� Value$# Center=�6S   �� 5y   &   " � ��
# �����6C   �� 5z   &   " � ��
# �����6D   �� 5{   &   " � ��$# 0.5,0.5,0.5,0q�6H   �� 5|   &   " � ��$# 4,5,4,5q�6@   5}   #   $ � 	Resources$ 5      *  $System.Windows.Controls.DataGridCell% �� Triggers& P� Property' P� Value��%�   '        �� 5�   "   "!  6(   % 5�   &   P� 5�   *   & �@ 
IsSelected' . . �5�   .   �� " � ��
# ��  �6K   �� 5�   .   " � ��
# ��·�6K   �� 5�   .   " � ��$# 0,0,0,0q�6P   5�   +   5�   #   5�      (   Columns( 5�      4  .System.Windows.Controls.DataGridTemplateColumn  5�   "   ,  &System.Windows.Controls.DataGridColumn)   Header$) O/S Code��69   *   SortMemberPath$* OutsoleCode��6K   +   ClipboardContentBinding+ 6h   �� *OutsoleCode+,   CellTemplate, 5�   &   �� 5�   *   .R�5�   .   �� $��SemiBold"�6U   $��Center=�6k   $��Center=�6�   $��2,0,2,0q�6�   ��68   �� *OutsoleCode+6�   5�   +   5�   #     5�   "   $) Delivery��69   $* QuantityDelivery��6K   + 6m   �� *QuantityDelivery+, 5�   &   �� 5�   *   .R�5�   .   �� $��Center=�6k   $��Center=�6�   $��2,0,2,0q�6�   ��68   �� *QuantityDelivery+- �� StringFormat$- N0��6�   5�   +   5�   #     5�   "   $) Checked��69   $* QuantityChecked��6J   + 6k   �� *QuantityChecked+, 5�   &   �� 5�   *   .R�5�   .   �� $��Center=�6j   $��Center=�6�   $��2,0,2,0q�6�   ��68   �� *QuantityChecked+$- N0��6�   5�   +   5�   #     5�   "   $) Reject��69   $%* QuantityReject, StringFormat=N0��6I   + 6z   �� *QuantityReject+, 5�   &   �� 5�   *   .R�5�   .   �� $��Center=�6X   $��Center=�6s   $��2,0,2,0q�6�   ��68   �� *QuantityReject+6�   5�   +   5�   #     5�   "   $) Return��69   $* QuantityReturn��6I   + 6i   �� *QuantityReturn+, 5�   &   �� 5�   *   .R�5�   .   �� $��Center=�6i   $��Center=�6�   $��2,0,2,0q�6�   ��68   �� *QuantityReturn+$- N0��6�   5�   +   5�   #     5�   "   $) 	Not Check��69   $* QuantityNotCheck��6L   + 6n   �� *QuantityNotCheck+, 5�   &   �� 5�   *   .R�5�   .   �� $��Center=�6j   $��Center=�6�   $��2,0,2,0q�6�   ��68   �� *QuantityNotCheck+$- N0��6�   5�   +   5�   #     5�   "   , 5�   &   �� 5�   *   .R�5�   .   �� $ btnViewDetail��65   -   5        #���  5�   L   $
 Hand��6�   $��Center=�6�   $��Center=�6�   $��2,0,2,0q�6�   . � ToolTip$. View Detail��6�   ��5�   2   �� 5�   6   $��14��6<   $��14��6G   $��/Images/large_chart.png��6S   5�   /   5�   +   5�   #   5�      � 5�      $��2��6    5�      �� 5�   "   $
��Auto �60   �� 5�   "   .+�5�      �  5�   "   �� 5�   &   $
��auto �67   �� 5�   &   .+�5�   "   �� -   5        / ��Name$/ tblTitleReportDetail��5�   ,   $��17(�6J   $��Cambria)�6X   $��SemiBold"�6m   �� 5�   "   $��1��6-   $��Right=�6=   $ 
Horizontal=�6Y   .+�5�   &   �� -   5        $ cboSupplierInventory��5�   /   $
 Name��6M   $ OutsoleSupplierId��6f   $��Center=�6�   6�   �� 5�   &   -   5        $
 txtPOFilter��5�   ?   $��2,0,0,0q�6.   $	��120��6T   
���   �$��Center=�6w   $��2,1,0,1q�6�   $��16(�6�   $. Input ProductNo��6�   �� 5�   &   -   5        $ 	btnFilter��5�   -   #���  6@   $
 Hand��6|   ��5�   *   �� 5�   .   $��18��64   $��18��6?   $��/Images/search.png��6K   5�   '   5�   #   5�        5�      -   5        $ dgDetail��5�   E   $��1��6'   $��0,2,0,0q�64    .  $ 2��6s   0   CanUserSortColumns0 .   .  $ FullRow=�5�   6   
 �����
 �����$	 NaNQ�6�    . $��Arial)�5�   R   $��12(�6e   1   FrozenColumnCount$1 2��6s     5�   "   �� 5�   &   "!  6,   .�5�   *   �� " � ��$# Center=�6W   �� 5�   *   " � ��
# �����6G   �� 5�   *   " � ��
# �����6H   �� 5�   *   " � ��$# 0.5,0.5,0.5,0q�6L   �� 5�   *   " � ��$# 4,5,4,5q�6D   5�   '   $ 5�   "   ��%�   '        �� 5�   &   "!  6,   % 5�   *   P� 5�   .   & �@ 
IsSelected' . . �5�   2   �� " � ��
# ��  �6O   �� 5�   2   " � ��
# ��·�6O   �� 5�   2   " � ��$# 0,0,0,0q�6T   5�   /   5�   '   5�   #   5�      5�      5�      5�      5�      5�      5�      