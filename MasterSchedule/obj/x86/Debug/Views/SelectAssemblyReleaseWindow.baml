   M S B A M L   `   `   `  ����p  lSystem.Windows.Controls.Input.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35pTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.ToolkitSystem.Windows.Controls  5   	    MasterSchedule6   0MasterSchedule.Views.SelectAssemblyReleaseWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   htoolkitTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.Toolkit     5   	   -   5          � Title$(  "Master Schedule - Select Report ID��5   	   $
��Auto��64   $	��300��6B    � SizeToContent$ Height=�6N    � WindowStartupLocation$ CenterScreen=�5   
    � 
ResizeMode$ CanMinimize=�6/   .��5      � $��5,5,5,5q�6    � ColumnDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��
Report ID:��6   $��Bottom=�6&   -   'System.Windows.Controls.AutoCompleteBox  5   
   -   5          Name$ txtReportId��5   "   $��1��67   $��5,0,0,0q�6G      Text$  ��6X   �� 5   
   $��1��6   $��1��6%    �� Orientation$ 
Horizontal=�62   $��Center=�6K   .+�5      �� -   5        	 ��Name$	 btnOk��5      $��75��6$   $��0,5,0,0q�6/   $��OK��6@   $
��Bold"�6N   
 �� 	IsDefault
 . {�.  6�   �� 5      -   5        $	 btnSearchExpand��5      $��75��6.   $��10,5,0,0q�69   $��	Search >>��6K   
 � Tag$ Search��6_   {�.  6~   5      � 5   
   -   5         �Name$ 
gridSearch��5      $��1��6#   $��2��63   $��5,5,0,0q�6@   $y�	Collapsed=�6Q    5      �� 5      $
��Auto �6#   �� 5      $��* �6#    5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5       $
��Auto �6    .+�5"      �� $��Product No.:��6   $��Bottom=�6,     5#      -   5        $ txtProductNo��5#   &   $��1��6<   $��5,0,0,0q�6L   $  ��6]   �� 5$      -   5        $	 	btnSearch��5$      $��1��6(   $��1��68   $��75��6E   $��0,5,0,0q�6P   $��Search��6a   6r   �� 5%      -   5         ��Name$ 
lvReportId��5%      $��2��6+   $��2��6?   $	��100��6L   $��0,5,0,0q�6Y   5&      5'      5(      5)      