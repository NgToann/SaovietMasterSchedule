   M S B A M L   `   `   `  ����g  cMicrosoft.ReportViewer.WinForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3atSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinFormsMicrosoft.Reporting.WinForms  5   	    MasterSchedule<   6MasterSchedule.Views.OutsoleMaterialRejectReportWindow   [rpvSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinForms  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $��13(�5   @     � Title$/  )Master Schedule - Outsole Material Report��5   	   $	��400��6;   $	��700��6H    � WindowState$ 	Maximized=�6T    � WindowStartupLocation$ CenterScreen=�6l   .��5      � $��3q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5	      .+�5   
   �  � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      .+�5      ��    GroupBoxTemplate#���   6   ��5      �� 5       �� Padding$ 4q�6     Background1#���  6)   ._�5      �� $��Show Report��6$   5      .��5      �  5      �� 5      �� 5      $��10 �6+   �� 5      �� 5      $��10 �6+   �� 5      �� 5      $��10 �6+   �� 5      .+�5       �� -   5         ��Name$ btnShowBalance��5       $��  _Show Balance  ��65   $��0��6n   �� 5!      -   5        $ btnShowReject��5!   -   $��1��6   $��  _Show Reject  ��6D   $��	15,0,15,0q�6{   $��0��6�   �� 5"      -   5        $ 
btnShowAll��5"   -   $��2��6   $��  _Show All  ��6A   $��0��6r   	� 5#      -   5         	�Name$ radShowBalance��5#   "   $��Balance��6:   $��Center=�6L    	� 	GroupName$ Option��6g   ��5$      �� 5%      	 �� 
TargetType"	 	�6$   
 �� Triggers
 5&   "   P� 5'   &    P� Property �@d�	IsChecked P� Value$
 TrueY�6O   . �5(   *   ��  �� Property � �� �� Value
 ��  �6G   �� 5)   *    � ��$
 Bold"�6G   5*   '   P� 5+   &    �@d�	IsChecked$ FalseY�6O   . �5,   *   ��  � ��
 �   �6G   �� 5-   *    � ��$ Normal"�6G   5.   '   50      52      	� 53      -   5        $ radShowReject��53   2   $��2��6"   $��Reject��6I   $��Center=�6Z   $ Option��6u    d� 	IsChecked$
 TrueY�6�   ��54      �� 55      "	 	�6$   
 56   "   P� 57   &    �@d�	IsChecked$
 TrueY�6O   . �58   *   ��  � ��
 ��  �6G   �� 59   *    � ��$
 Bold"�6G   5:   '   P� 5;   &    �@d�	IsChecked$ FalseY�6O   . �5<   *   ��  � ��
 �   �6G   �� 5=   *    � ��$ Normal"�6G   5>   '   5@      5B      	� 5C      -   5        $ 
radShowAll��5C   2   $��4��6"   $��Balance and Reject��6F   $��Center=�6c   $ Option��6~   ��5D      �� 5E      "	 	�6$   
 5F   "   P� 5G   &    �@d�	IsChecked$
 TrueY�6O   . �5H   *   ��  � ��
 ��  �6G   �� 5I   *    � ��$
 Bold"�6G   5J   '   P� 5K   &    �@d�	IsChecked$ FalseY�6O   . �5L   *   ��  � ��
 �   �6G   �� 5M   *    � ��$ Normal"�6G   5N   '   5P      5R      	� 5S      -   5        $ radAssemblyReject��5S   2   $��6��6"   $��Assembly / Stockfit��6M   $��Center=�6k   $ Option��6�   ��5T      �� 5U      "	 	�6$   
 5V   "   P� 5W   &    �@d�	IsChecked$
 TrueY�6O   . �5X   *   ��  � ��
 ��  �6G   �� 5Y   *    � ��$
 Bold"�6G   5Z   '   P� 5[   &    �@d�	IsChecked$ FalseY�6O   . �5\   *   ��  � ��
 �   �6G   �� 5]   *    � ��$ Normal"�6G   5^   '   5`      5b      5c      5d      5e      7  1System.Windows.Forms.Integration.WindowsFormsHost  5f   
   $��1��6   $��0,5,0,0q�6(      Child. 5g      /   )Microsoft.Reporting.WinForms.ReportViewer  -	   5        reportViewer��   ShowFindControls .  5g   5   5h      5i      5j      