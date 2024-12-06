# R8 Control

#### <img src="New32x32.png" alt="New" title="Something new has happened." width="24" height="24" /> A New Resource is Ready to Deploy
We released version 1.1.1 of **R8 Control** today (12/6/24) and made
a few changes in our release process. You will notice that we released two
types of packages for installing the application. One of the packages
is a regular zip file containing all the files needed to run **R8C**.
That's how the program has been released up until now. We have also
added a new Inno Setup install file that a user may run on his
desktop to automate the installation process.

The runtime files in both packages are identical, so the user can
choose either to install **R8C** on his system.

We plan to check out ways to sign the packages so users can trust
that the packages come from us and haven't been modified. It took
us a while to get to this point because the automated deployment
we implemented presented us with many twists and turns. Hopefully,
the next step won't be quite as difficult

---
**R8 Control** (R8C) is an external Run 8 controller offering precise throttle
control and many other features. It is designed for use on multiple-screen
systems where Run 8 is viewed in full-screen mode, and **R8C** is viewed
on another. However, it is possible to view Run 8 in window mode
and have both programs operating on a single screen.

![image](https://github.com/user-attachments/assets/cdd1fd84-583f-4328-b303-4ec78cb61f7c)

Run 8 already supports Rail Driver and has a keyboard and
mouse interface. Why should you consider using **R8C** with Run 8?
There are several reasons. First and foremost, Rail Driver
and keyboard inputs are imprecise, making fine control of
the train throttle and brakes difficult. **R8C**, on the other hand,
allows precise increments and decrements of the throttle and
brake controls, making it much easier to find "sweet spots" where
everything balances out and trains run themselves.

![image](https://github.com/user-attachments/assets/7614e9a6-cdce-4041-99bc-4fa3627129a8)

Each slider control in the Driver 1 window (except for the Reverser) can be
adjusted by increments of 1 out of 256 possible values. That's as precise as it gets
and is impossible within Run 8 alone.

Also, keyboard shortcuts can be complex to remember, especially where
many exist. You will still use some shortcuts
while using **R8C**, but less often. **R8C** has buttons
and sliders for all of the following functions (plus some):

* The throttle.
* Dynamic brakes.
* Air brakes (aka train brakes).
* The independent (Indy) brakes.
* The reverser.
* The parking brakes.
* The horn.
* The bell.
* Alert recognition.
* Slow speed start and stop.
* Set and release the parking brakes.
* The sander.
* EOT Emergency Stop.
* All of the auto start/configuration options.
* All of the manual start/configuration options (for realism).
* DPU controls.
* Traction Motors.
* Radio controls.
* Light controls.

You saw some of the controls for these functions above. Here are
The other input controls are laid out for you to see.

![image](https://github.com/user-attachments/assets/9758394a-acba-4892-a6f5-e2964363e6fb)

You might also appreciate seeing the previous signal's
type and the next signal's type. That's not available even
in Run 8 by itself.

In addition to these and other features, you will find in
**R8C**, the program is free and open source. When I tire of
updating the code to keep up with Run 8 and add new features,
another developer may come along with his own branch and pick
up where I've stopped.

So, download a copy for yourself, and enjoy!
