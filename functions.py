#chain
def calc(calc_p):
    class Holder:
        v = None

    def show_status(func):
        def inner():
            print('executing {0} current value is {1}'.format(func, Holder.v))
            return func()

        return inner

    l, nums = calc_p
    result = []
    def context(n):
        Holder.v = n

        @show_status
        def m1(): Holder.v = Holder.v + l(1)
        @show_status
        def m2(): Holder.v = Holder.v * l(10)
        @show_status
        def m3(): Holder.v = Holder.v + l(100)

        [m() for m in [m1, m2, m3]]
        result.append(Holder.v)

    list(map(context, nums))
    print(result)

l = input("enter lambda to use:")
calc((eval(l), [10, 11, 13]))



#[l(10) for l in c]


#from functools import reduce
#product = reduce((lambda x, y: x * y), )

#print (product)