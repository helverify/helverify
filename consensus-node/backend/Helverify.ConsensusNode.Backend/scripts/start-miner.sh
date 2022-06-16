#!/bin/sh

ethdir=/home/eth
bootnode=`cat $ethdir/bootnode`

geth -datadir $ethdir/data init $ethdir/genesis.json > $ethdir/eth.log 2>&1

geth --datadir $ethdir/data --networkid 13337 --bootnodes $bootnode --mine