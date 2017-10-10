use UsersDatabase
/* delete all roles for Peter Puglisi accept Aothor */
Delete from AspNetUserRoles
Where UserId = '38e75294-d0df-4f8c-ae86-763a70df9117'
And RoleId != '3ceda92d-ef91-44ae-a857-084175aff81a'

/* Peter Puglisi applicaiton user */
Select * from AspNetUserRoles
Where UserId = '38e75294-d0df-4f8c-ae86-763a70df9117'

/* delete all applicaiton user roles */
Delete from AspNetUserRoles
Where UserId = '38e75294-d0df-4f8c-ae86-763a70df9117'
And RoleId != '3ceda92d-ef91-44ae-a857-084175aff81a'

Mirko Change as part of the article 14
