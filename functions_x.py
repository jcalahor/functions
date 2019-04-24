#chain
import random
from statistics import mean

class InputClass(object):
    def __init__(self, val):
        self.val = val

def calc(calc_p):

    class Holder:
        v = None

    def show_status(func):
        def inner(*original_args, **original_kwargs):
            print('{0} current value is {1}'.format(func, Holder.v))
            return func(*original_args, **original_kwargs)
        return inner

    @show_status
    def str_action(n):
        exec(n, globals())
        Holder.v = dummy_f()

    @show_status
    def input_class_action(n):
        Holder.v = n.val

    @show_status
    def function_action(n):
        Holder.v = n(random.random() * 100)

    @show_status
    def number_action(n):
        Holder.v = n

    @show_status
    def tuple_action(n):
        action, values = n
        Holder.v = action(values)

    ACTION_MAP = {
        'float': number_action,
        'int': number_action,
        'str': str_action,
        'InputClass': input_class_action,
        'function': function_action,
        'tuple': tuple_action
    }

    l, nums = calc_p
    result = []

    def context(n):
        t = type(n)
        print (t.__name__)
        ACTION_MAP[t.__name__](n)

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

user_lambda = input("enter lambda to use:")
calc((eval(user_lambda), [10.54, 34, 'def dummy_f(): return 100', InputClass(8980), lambda x: x + 900, (mean, [6,7,12])]))
