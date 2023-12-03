# Growthware
Growthware is an idea dedicated to producing reusable and extendable core technologies providing role based security with dynamic menus and navigation.  


<b>Welcome to the Growthware source control site.</b>
<p>
    Growthware is an idea dedicated to producing reusable and extendable core technologies used to produce a working web application in less than 10 minutes.  The framework has now grown to work with both .Net 4.8 and .Net Core 8.  Not all features are avalible in all incarnations so they will denoted with 4.8 or 8 for each version.
</p>
The framework was developed to provide a data store independent/generic code base to supply the following functionality and features.
<table>
    <caption>Account Management</caption>
    <thead>
        <tr>
            <td>Feature</td>
            <td>.Net 4.8 VB</td>
            <td>.Net 4.8 C#</td>
            <td>.Net Core 8 C#</td>
            <td>UI</td>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Creation</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 17</td>
        </tr>
        <tr>
            <td>Registration - Internal</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS</td>
        </tr>
        <tr>
            <td>Registration - Google</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS</td>
        </tr>
        <tr>
            <td>Registration - Facebook</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS</td>
        </tr>
        <tr>
            <td>Registration - Twitter</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS</td>
        </tr>
        <tr>
            <td>Registration - Microsoft Account</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS</td>
        </tr>
        <tr>
            <td>Edit (including the ability for each client to edit their personal information)</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS</td>
        </tr>
        <tr>
            <td>Authentication - Internal</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular</td>
        </tr>
        <tr>
            <td>Authentication - LDAP/ADSI</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS</td>
        </tr>
        <tr>
            <td>Authentication - IIS A.K.A Windows Authentication</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS</td>
        </tr>
    </tbody>
</table>
<ul>
    <li>An E-Mail facility </li>
    <li>Logging using Log4Net </li>
    <li>
        Security Entity Management
        <ul>
            <li>Manage properties of a Security Entity </li>
            <li>Add a Security Entity through the UI </li>
            <li>Unlimited number of Parent/Child Security Entitys </li>
        </ul>
    </li>
    <li>
        Role management
        <ul>
            <li>Adding a role </li>
            <li>Editing a role </li>
            <li>Deleting a role </li>
        </ul>
        Note: Role management is by Security Entity, roles are created and assigned to Page/Functions for 4 “Permissions” (View, Add, Edit, and Delete). Roles are also assigned to Accounts. When both the Client/Account and Function/Page for any given permission has the same role then that permission is granted. The role based security is accumulative meaning if the account is in a any role that has a permission then the permission is granted.
    </li>
    <li>
        Function/Page Management
        <ul>
            <li>Add Functions/Pages </li>
            <li>Edit Functions/Pages </li>
            <li>Delete Functions/Pages </li>
            <li>Security Management – Assigning roles by Security Entitys </li>
        </ul>
    </li>
    <li>
        Client Choice Management
        <ul>
            <li>As a client makes choices in the application the information is stored in the data store and retrieved when necessary. </li>
        </ul>
    </li>
    <li>
        Personalization
        <ul>
            <li>Choose from five color schemas </li>
            <li>Select your favoriate action </li>
            <li>Choose the number of records to show per page </li>
        </ul>
    </li>
    <li>
        Skinning
        <ul>
            <li>Skinning is the ability to put any look and feel to the application without changing the underpinnings of the application code. </li>
            <li>Skins can be applied on the fly </li>
            <li>Skins are setup on a per Security Entity basis, so when a Security Entity is selected the look an feel change to the Security Entitys skin. </li>
            <li>Skins make an excellent visual queue making it easier to identify the Security Entity your working with </li>
        </ul>
    </li>
    <li>
        Quick development
        <ul>
            <li>When using the web application as a staring point the setup time should be less than a single day, after the setup time all efforts can be concentrated on the business logic at hand saving precious development time. </li>
            <li>Applications starting with the Web Application start with a given level of <b>Q</b>uality <b>A</b>ssurance and behavior </li>
        </ul>
    </li>
</ul>
