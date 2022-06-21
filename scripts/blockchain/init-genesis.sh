#!/bin/sh

ethdir=/home/eth

geth --datadir $ethdir/data init $ethdir/genesis.json > $ethdir/eth.log 2>&1