   M S B A M L   `   `   `  ����g  cMicrosoft.ReportViewer.WinForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3atSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.ReportViewer.WinFormsMicrosoft.Reporting.WinForms  5   	    MasterSchedule3   -MasterSchedule.Views.LeadTimePerSectionWindow   [rpvSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.ReportViewer.WinForms  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$8  2Master Schedule - Lead Time Per Section Statistics��5   	   $	��500��6D   $
��1000��6Q    � WindowState$ 	Maximized=�6^    � WindowStartupLocation$ CenterScreen=�6v   .��5      � $��5q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      .+�5   
   ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��Date:��6   $��14(�6%   $��Bottom=�63   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ 
dpDateFrom��5      $��5,0,0,0q�6-      SelectedDate$ 	1/16/2018X�6>   �� 5      $
�� to ��6   $��14(�6$   $��Bottom=�62     5      -   5        $ dpDateTo��5      $ 	1/16/2018X�6+   �� 5      -   5         ��Name$ 
cboSection��5      $	��100��6+   $
��Bold"�6:   $��10,0,0,0q�6L   $��14(�6^   @�5      �� 5      -   5        $��S��5   4   $��Sewing��6#   6?   �� 5      -   5        $��A��5   6   $��Assembly��6#   6A   �� 5      -   5        $��O��5   5   $��Outsole��6#   6@   �� 5      -   5        $��SL��5   8   $��
Socklining��6#   6D   5       �� 5!      -	   5         ��Name$ btnCreateChart��5!      $��40,0,0,0q�6-   $��Create Chart��6?   $	��100��6V   $��14(�6e   6s   5"      � 5#   
   $��1��6   $��0,5,0,0q�6   .+�5$      7  1System.Windows.Forms.Integration.WindowsFormsHost  	   Child.	 5%      /   )Microsoft.Reporting.WinForms.ReportViewer  -
   5        reportViewer��
   ShowFindControls
 .  5%   9   5&      5'      5(      5)      