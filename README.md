# Growthware
Growthware is an idea dedicated to producing reusable and extendable core technologies providing role based security with dynamic menus and navigation.  


<b>Welcome to the Growthware source control site.</b>
<p>
    The Growthware code base is in essence used to produce a functioning web application/site handling Role based security.  The web application/site code provides the ability to manage Roles, Groups, Accounts, Functions and Security Entities.  Supported middle tier technologies are written with both .Net 4.8 and .Net Core 9.  Not all features are avalible in all incarnations so they will denoted with 4.8 or 9 for each version.
</p>
<p>
The framework was developed to provide a data store independent/generic code base where SQL Server has been implemented.
</p>
<p>
The following is a summary of the features available.
</p>

<table>
    <caption>Account Management</caption>
    <thead>
        <tr>
            <td>Feature</td>
            <td>.Net 4.8 VB</td>
            <td>.Net 4.8 C#</td>
            <td>.Net Core 9 C#</td>
            <td>UI</td>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Creation</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>Registration - Internal</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>Registration - Google</td>
            <td>X</td>
            <td>X</td>
            <td> </td>
            <td>ASPX, Angular JS</td>
        </tr>
        <tr>
            <td>Registration - Facebook</td>
            <td>X</td>
            <td>X</td>
            <td> </td>
            <td>ASPX, Angular JS</td>
        </tr>
        <tr>
            <td>Registration - Twitter</td>
            <td>X</td>
            <td>X</td>
            <td> </td>
            <td>ASPX, Angular JS</td>
        </tr>
        <tr>
            <td>Registration - Microsoft Account</td>
            <td>X</td>
            <td>X</td>
            <td> </td>
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
            <td></td>
            <td>ASPX, Angular JS</td>
        </tr>
        <tr>
            <td>Authentication - IIS A.K.A Windows Authentication</td>
            <td>X</td>
            <td>X</td>
            <td> </td>
            <td>ASPX, Angular JS</td>
        </tr>
    </tbody>
</table>
<table>
    <caption>Security Management</caption>
    <thead>
        <tr>
            <td>Feature</td>
            <td>VB</td>
            <td>C#</td>
            <td>.Net 4.8</td>
            <td>.Net Core 9</td>
            <td>UI</td>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Add, Edit, Security Entities (SE)</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>Add, Edit, Delete, Roles by SE</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>Add, Edit, Delete, Groups by SE</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>Add, Edit, Delete, Functions</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>Associate Roles/Groups to Functions/Pages</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>Associate Roles/Groups to Accounts</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
    </tbody>
</table>
<table>
    <caption>Miscellaneous</caption>
    <thead>
        <tr>
            <td>Feature</td>
            <td>VB</td>
            <td>C#</td>
            <td>.Net 4.8</td>
            <td>.Net Core 9</td>
            <td>UI</td>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Database Manager</td>
            <td></td>
            <td></td>
            <td></td>
            <td>X</td>
            <td>Console</td>
        </tr>    
        <tr>
            <td>An E-Mail facility</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>N/A</td>
        </tr>
        <tr>
            <td>Message Management</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>Logging - File</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>N/A</td>
        </tr>
        <tr>
            <td>Logging - Datastore</td>
            <td></td>
            <td></td>
            <td></td>
            <td>X</td>
            <td>N/A</td>
        </tr>
        <tr>
            <td>Client Choices</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>Multi-Tenant</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>Multiple datastore technology</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>File Manager</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX, Angular JS, Angular 18.2</td>
        </tr>
        <tr>
            <td>In-memory storage (Cache/Session)</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>N/A</td>
        </tr>
        <tr>
            <td>Community Calendar (Demonstrates Multi-Tenant support)</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>X</td>
            <td>ASPX</td>
        </tr>
    </tbody>
</table>
<ul>
    <li>
        Role/Group Management: Roles are created and assigned to Page/Functions for 4 “Permissions” (View, Add, Edit, and Delete). Roles are also assigned to Accounts. When both the Client/Account and Function/Page for any given permission has the same role then that permission is granted. The role based security is accumulative meaning if the account is in a any role that has a permission then the permission is granted.
    </li>
    <li>
        Group Management: Roles are associated with Groups and or other Groups.  It can help to think of Groups as a "Job" where someone does more than just 1 thing.
    </li>
    <li>
        Client Choice Management: As a client makes choices in the application the information is stored in the data store and retrieved when necessary.
        <ul>
            <li>Records per page</li>
            <li>
                Choose an SE
            </li>
            <li>
                Choose a color scheme
            </li>
            <li>
                Favorite "Action"
            </li>
        </ul>
    </li>
    <li>
        Multiple datastore technologies is supported but not implemented an example can be found in .Net 4.8 VB/CS
    </li>
    <li>
        Multi-Tenant Support exist in all flavors of Growthware.
        <ul>
            <li>
                The Security Entity (SE) is the mechanism by which multi-tenant support is achieved in the datastore and UI.
            </li>
            <li>
                A "Skin" is associated with an SE and is how the UI can be completely different for each one.  Note that a URL can also be associated with the SE and could faciliate have a unique UI displayed via a given URL, though this is not implemented it wouldn't take much to implement.
            </li>
            <li>
                Associate an SE in a data table to allow data to be stored in a single DB instance for a given "tenant"
            </li>
            <li>
                With a connection string, Data Access (DAL), a "Central Management" boolean associated with an SE, it is possible to completely separate the data into, not only a different datastore, but, a different datastore technology!
            </li>
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
