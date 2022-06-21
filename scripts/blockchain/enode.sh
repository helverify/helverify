#!/bin/sh

ethdir=/home/eth

until [ -e $ethdir/data/geth.ipc ]
do
    sleep 5
done

geth attach $ethdir/data/geth.ipc --exec admin.nodeInfo.enode > $ethdir/enode
