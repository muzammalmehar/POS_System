# Arslan Communication & Karyana Store User Manual

## 1. Introduction

`Arslan Communication & Karyana Store` uses this Windows Forms based point-of-sale and shop management system for:

- Grocery sales
- Customer credit tracking
- Vendor purchase and payable tracking
- Stock and product management
- Expiry management
- Service transactions such as bill payment, mobile balance, packages, and cash withdrawal
- Accounts, profit and loss, and printable records

This manual is written for daily shop use and covers both normal and edge-case workflows.

## 2. System Modules

The main dashboard gives access to these modules:

- Login
- Dashboard
- Sales Screen
- Stock Management
- Product Management
- Purchase Entry
- Vendor Management
- Vendor Payments
- Expense Management
- Expiry Management
- Service Center
- Customer Management
- Accounts and P&L
- Recent Sales

## 3. Login

### Purpose

Login controls access to the system and opens the main dashboard for the current user.

### Features

- Username and password login
- Remember Me option
- Saved login details for faster future login

### How to use

1. Open the application.
2. Enter username.
3. Enter password.
4. If you want the system to remember your login, tick `Remember me`.
5. Click `Sign In`.

### Notes

- If `Remember me` is enabled, the system saves login details locally for that Windows user.
- If login fails, saved credentials are cleared to prevent wrong repeated login.

## 4. Dashboard

### Purpose

The dashboard is the home screen of the software and gives quick business information.

### Typical information shown

- Today grocery sales
- Today grocery profit
- Today credit sales
- Today cash sales
- Today service sales
- Today service profit
- Today expenses
- Today orders
- Low stock alerts
- Recent sales list

### What to check daily

- Whether credit sales are growing too much
- Which items are low in stock
- Whether there are expiry alerts
- Whether expenses are high compared to daily profit

## 5. Product Management

### Purpose

Used to create and update products.

### Available fields

- Product Code
- Barcode
- Product Name
- Category
- Brand
- Base Unit
- Preferred Vendor
- Purchase Price
- Sale Price
- Reorder Level
- Track Stock
- Track Expiry
- Expiry Date
- Product Image
- Active Status

### Important expiry rule

Product-level expiry date is a default/reference expiry setting.
Actual stock expiry for a purchased batch should still be added during purchase entry.

### How to add a new product

1. Open `Product Management`.
2. Click `New Product`.
3. Enter product name.
4. Fill barcode if available.
5. Select category, brand, and unit.
6. Select preferred vendor if known.
7. Enter purchase price and sale price.
8. Enter reorder level.
9. Keep `Track stock` enabled for normal inventory items.
10. Enable `Track expiry` if the item expires.
11. If expiry applies, select the product expiry date.
12. Add image by:
    - `Choose` from local system, or
    - `Capture` from camera
13. Click `Save Product`.

### Editing a product

1. Search the product in the left grid.
2. Select it.
3. Update fields.
4. Save again.

### Best practices

- Keep sale price and purchase price updated.
- Assign the correct vendor for purchase and expiry return flow.
- Enable expiry tracking only for products where expiry really matters.

## 6. Stock Management

### Purpose

Used to review stock levels and perform stock adjustments.

### Typical actions

- Search product stock
- View current stock
- View stock value
- View stock movement history
- Post adjustments such as:
  - Opening stock
  - StockAdjustIn
  - StockAdjustOut
  - Damage

### When to use it

- When physical stock and system stock do not match
- When opening old business records in the software
- When damaged stock must be removed

## 7. Expiry Management

### Purpose

Used to handle near-expiry and expired products.

### Main functions

- Show expiring products
- Show expired stock records
- Move expired stock out of active stock
- Mark expired stock as:
  - Pending
  - ReturnedToVendor
  - Burnt
  - Adjusted

### Full expiry workflow

1. Product is marked as expiry-tracked.
2. Stock is purchased with actual batch expiry date.
3. Sales consume valid stock first.
4. Once expired, stock is moved to expired records.
5. Expired stock can then be:
   - sent back to vendor
   - burnt
   - adjusted manually

### Important note

Expired stock should not remain in normal sellable stock.

## 8. Purchase Entry

### Purpose

Used to buy stock from vendors and add that stock into the system.

### Main fields

- Vendor
- Wallet
- Purchase Date
- Invoice No
- Discount
- Other Charges
- Paid Amount
- Remarks
- Product cart

### Per product line fields

- Quantity
- Cost
- Batch
- Expiry

### Credit support

Purchase entry supports all three conditions:

- Full paid purchase
- Partial payment purchase
- Full credit purchase

### How to make a normal purchase

1. Open `Purchase Entry`.
2. Select vendor.
3. Search product.
4. Add selected product.
5. Enter quantity and cost.
6. If expiry applies, enter:
   - Batch
   - Expiry date
7. Enter invoice no if available.
8. Enter paid amount.
9. Select wallet if any amount is paid now.
10. Save purchase.

### How partial vendor credit works

Example:

- Total purchase = `Rs. 10,000`
- Paid now = `Rs. 4,000`
- Remaining payable = `Rs. 6,000`

System behavior:

- wallet is reduced by `Rs. 4,000`
- stock is added
- purchase header saves full total
- remaining amount is saved as vendor due
- vendor payable screen shows the remaining amount

### Expiry in purchase

If a product tracks expiry:

- the row is highlighted
- cursor moves directly to expiry entry
- expiry can be selected using date picker

## 9. Vendor Management

### Purpose

Used to manage suppliers, their payment schedules, linked products, purchases, and payables.

### Vendor details include

- Vendor name
- Phone
- Address
- Opening balance
- Visit day
- Payment cycle
- Credit days
- Next payment date
- Notes
- Linked products

### Vendor payable view includes

- Net balance
- Purchase due
- Payment history
- Purchase history
- Ledger
- Expired/returned product history

### Vendor create/edit

Vendor creation is separated into its own form so the management screen stays clean.

### Vendor linked products

Vendor and product can be linked from both sides:

- while creating/editing vendor
- while creating/editing product

## 10. Vendor Payments

### Purpose

Used to pay vendor against payable balance.

### How it works

1. Select vendor.
2. Enter payment amount.
3. Select wallet.
4. Add remarks if needed.
5. Save payment.

### Result

- vendor payable reduces
- wallet decreases
- accounting voucher is posted
- printable payment receipt can be previewed or printed

### Printing

Vendor payment receipts support:

- print preview
- direct print
- A4 template

## 11. Customer Management

### Purpose

Used to manage customers, credit sales, payments received, history, and ledger.

### Main functions

- Add customer
- Edit customer
- View purchase history
- View payment history
- View combined ledger
- Receive customer payment
- Print customer payment receipt

### Customer data

- Name
- Phone
- Address
- Opening balance
- Balance type
- Image
- Active status

### Customer creation

Customer creation is separated into its own form.

### Customer payment flow

If a customer pays an old due:

- customer balance decreases
- wallet increases
- customer ledger updates
- printable receipt is available

## 12. Sales Screen

### Purpose

Used for grocery billing and point-of-sale activity.

### Available features

- Product search
- Keyboard navigation with up/down keys
- Enter to add product
- Cart editing
- Quantity editing
- Remove product from cart
- New customer from sale screen
- Stock button inside sale screen
- Payment method selection
- Wallet selection
- Discount
- Extra charges
- Paid amount
- Credit due display

### Supported sale types

- Full paid sale
- Full credit sale
- Partial credit sale

### Important credit rule

If customer still owes money after sale, customer must be selected.

### Example of partial sale

- Grand total = `Rs. 2,000`
- Customer pays = `Rs. 800`
- Remaining due = `Rs. 1,200`

System behavior:

- sale is saved
- wallet receives `Rs. 800`
- customer due becomes `Rs. 1,200`
- history and ledger reflect the due

### Cashier tips

- Scan or type product and press `Enter`
- Use `Up` and `Down` to move in product list
- After product add, focus returns for next product
- Quantity `0` removes cart row

## 13. Recent Sales

### Purpose

Used to view, filter, print, edit, and refund past grocery and service records.

### Functions

- Combined grocery and service records
- Filter by customer
- Filter by type
- Edit a selected record
- Refund a selected record
- Print a single record
- Print multiple selected records

### Printing

Recent sales printing is designed for customer records.

It includes:

- customer details
- payment details
- status
- disclaimer

It does not show internal profit or commission details.

### Disclaimer format

Printed reports include wording similar to:

`This report is generated by {user} on the demand of customer.`

## 14. Service Center

### Purpose

Used for non-grocery service transactions.

### Typical services

- Mobile balance
- Data packages
- Bill payment
- Cash withdrawal

### Important business logic

- Commission is your earning
- Service amount is not always your profit
- Customer service history should remain searchable

### Features

- Service entry
- Customer tracking for repeat monthly bills
- Reference/consumer number tracking
- Wallet selection
- Recent service history
- Tracked customer profiles

### Monthly bill customers

Use customer profile and service history to track repeat bill payments every month.

## 15. Expense Management

### Purpose

Used to record business expenses.

### Examples

- Electricity bill
- Rent
- Salary
- Delivery expense
- Repairs
- Transport

### Effect

- expense is stored
- accounting is updated
- expense appears in dashboard and P&L

## 16. Accounts and Profit & Loss

### Purpose

Used to view balances and business performance.

### Shows

- account balances
- recent accounting vouchers
- grocery sales
- grocery profit
- service sales
- service profit
- date-wise P&L

### Important understanding

- Grocery sales profit is based on sale versus cost of goods sold
- Service profit is based on commission/earning logic

## 17. Credit and Partial Payment Rules

This is one of the most important parts of the system.

### Sales

- Full paid:
  - paid amount = full amount
  - no customer due
- Full credit:
  - paid amount = `0`
  - customer due = full total
- Partial credit:
  - paid amount is between `0` and full total
  - customer due = remaining amount

### Purchases

- Full paid:
  - vendor due = `0`
- Full credit:
  - paid amount = `0`
  - vendor due = full total
- Partial payment:
  - some amount paid now
  - rest remains vendor payable

### Wallet behavior

- customer payment received increases wallet
- grocery sale payment received increases wallet
- purchase payment to vendor decreases wallet
- vendor payable payment decreases wallet

## 18. Images

### Product image

Can be:

- selected from local system
- captured live from camera

### Customer image

Can be:

- selected from local system
- captured live from camera

### Storage

Images are copied into local application folders for reuse.

## 19. Printing

Current printing support includes:

- vendor payment receipt
- customer payment receipt
- vendor statement
- recent sales record print

### Print styles

- A4 template
- preview before print
- customer-facing clean layout

## 20. Refunds and Editing

### Grocery sale editing

Recent sales allows selected sales to be edited.

### Refunds

Refund updates:

- sale/service status
- stock if applicable
- wallet reversal if applicable
- accounting reversal if applicable

## 21. Suggested Daily Workflow

### Start of day

1. Login
2. Check dashboard
3. Review low stock
4. Review expiry alerts

### During the day

1. Make sales
2. Add customer if credit is needed
3. Record services
4. Record expenses
5. Record vendor purchases if stock arrives

### End of day

1. Review dashboard
2. Review recent sales
3. Check customer dues
4. Check vendor payables
5. Check accounts summary

## 22. Suggested Weekly Workflow

1. Review customer outstanding balances
2. Review vendor due list
3. Review expired stock pending action
4. Review low-stock items
5. Review profit and loss

## 23. Troubleshooting

### Login fails

Check:

- database connection string
- MySQL server is running
- database exists
- username/password are correct

### Product not visible in sales or purchase

Check:

- product is active
- correct vendor is selected in purchase
- search text is correct

### Credit sale error asking for customer

This happens when:

- paid amount is less than grand total
- no customer is selected

Fix:

- select customer before saving

### Purchase payment error

If any amount is paid now, a wallet must be selected.

### Expiry save error

If product tracks expiry, purchase line must contain expiry date.

### Designer issues in Visual Studio

If form designer does not open:

1. close the designer tab
2. build project
3. reopen designer

## 24. Important Operational Advice

- Always assign customer for credit sales
- Always assign actual expiry date at purchase level
- Keep vendor-product links updated
- Use recent sales print when customer asks for record
- Do not leave expired stock in normal inventory
- Record partial payments properly instead of adjusting manually outside the system

## 25. Future Manual Expansion

This manual can later be extended with:

- screenshots of each form
- cashier training section
- admin training section
- backup and restore guide
- cloud deployment guide
- printer setup guide

---

Prepared for `Arslan Communication & Karyana Store` as an operational guide for shop staff, cashier, owner, and manager use.
