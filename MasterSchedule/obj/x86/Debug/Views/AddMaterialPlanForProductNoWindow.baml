   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.3.0.4, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views  5   	   <    6MasterSchedule.Views.AddMaterialPlanForProductNoWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   :    � SizeToContent$ WidthAndHeight=�5       � 
ResizeMode$ CanMinimize=�69   $��13(�6R    � Title$> 8Master Schedule - Add/Update Material Plan For ProductNo��5   	   $	��550��6J   .��5	      � $��2q�6    � RowDefinitions 5
   
   �� 5      $
��auto �6   �� 5      $��2 �6   �� 5      $
��auto �6   �� 5      $��5 �6   �� 5      $
��auto �6   �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      �� 5      $��5 �6   �� 5      �� 5      $��5 �6   �� 5      .+�5   
   �� $��Accessory Name��6   �� 5   
   $��2��6   $��Supplier��6$   �� 5   
   $��4��6   $	��ETD��6$   �� 5   
   -   5         ��Name$ cboAccessoryName��5       $��2��6   $��Center=�6:   6\   �� 5   
   -   5        $ cboSupplierName��5   0   $��2��6   $��2��6     �� DisplayMemberPath$
 Name��6I    �� SelectedValue$ 
SupplierId��6b   $��Center=�6}   (  "System.Windows.Controls.DatePicker  5   
   -   5        	  Name$	 dpETD��5      $��2��6$   $��4��61   $��Center=�6A   �� 5    
   $��Remarks��6   $��4��6#   $��Italic$�60   
����  �6C   �� 5!   
   -   5        
 ��Name$
 
txtRemarks��5!      $��5��6&   $��3��63   $��40��6G   
���  �� �� TextWrapping$
 Wrap=�6d   
���   �$��2q�6�   � 5#   
   $��6��6   $��6��6    5$      �� 5%      $��* �6    �� 5&      $
��auto �6     5(      �� 5)      �� 5*      $��10 �6#   �� 5+      .+�5-      �� -   5         ��Name$ 	btnDelete��5-   "   $��1��6      ButtonTemplate#���   65   $��Delete��6m   {�.  6�   �� 5.      -   5        $ btnSave��5.   "   $��1��6   #���   63   $��2��6k   $
��Save��6{   6�   5/      50      51      