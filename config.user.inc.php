<?php

$cfg['blowfish_secret'] = getenv('PMA_SECRET');

/* ----------------------- SERVER 2 : LOCALHOST ---------------------- */

$i = 0;
$i++;
$cfg['Servers'][$i]['verbose'] = 'Local MySQL';
$cfg['Servers'][$i]['host']     = getenv('LOCAL_HOST');
$cfg['Servers'][$i]['port']     = '3306';
$cfg['Servers'][$i]['auth_type'] = 'config';
$cfg['Servers'][$i]['user']     = getenv('LOCAL_USER');
$cfg['Servers'][$i]['password'] = getenv('LOCAL_PASSWORD');
$cfg['Servers'][$i]['AllowNoPassword'] = true;

/* ------------------------- SERVER 1 : AWS ------------------------- */

$i++;
$cfg['Servers'][$i]['verbose'] = 'AWS RDS';
$cfg['Servers'][$i]['host']     = getenv('AWS_HOST');
$cfg['Servers'][$i]['port']     = '3306';
$cfg['Servers'][$i]['auth_type'] = 'config';
$cfg['Servers'][$i]['user']     = getenv('AWS_USER');
$cfg['Servers'][$i]['password'] = getenv('AWS_PASSWORD');

/* ------------------------- DEFAULT SETTINGS ------------------------ */

$cfg['UploadDir'] = '';
$cfg['SaveDir'] = '';